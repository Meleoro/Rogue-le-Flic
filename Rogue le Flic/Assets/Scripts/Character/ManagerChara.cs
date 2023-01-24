using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Pathfinding;
using UnityEngine.SocialPlatforms.Impl;

public class ManagerChara : MonoBehaviour
{
    public static ManagerChara Instance;
    
    [HideInInspector] public Rigidbody2D rb;
    public GameObject activeGun;
    public GameObject stockWeapon;

    public Controls controls;
    [HideInInspector] public bool noControl;
    public bool munitionsActives;
    
    public bool isInTuto;

    [Header("Dash")] 
    [HideInInspector] public bool isDashing;
    private float timerDash;
    
    [Header("Kick")] 
    private bool isKicking;
    private float timerKick;
    public bool keyboardDirection;

    [Header("Repositionnement")] 
    public Vector2 posLeft;
    public Vector2 posRight;

    [Header("Death")] public Image fondMort;

    [Header("Autres")] 
    public float switchCooldown;
    public bool canSwitch;
    public Animator anim;
    public Image reload;
    [HideInInspector] public bool isFalling;
    [HideInInspector] public Vector2 savePosition;

    private bool doOnce;
    public GameObject test;


    public AudioSource death;
    public AudioSource fall;
    public AudioSource spot;

    public GameObject spotSprite;
    
    



    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 

        rb = GetComponent<Rigidbody2D>();
        
        controls = new Controls();
    }
    
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        canSwitch = true;
        
        reload.GetComponentInParent<Canvas>().enabled = false;
        
        
        MenuMortManager.Instance.gameObject.SetActive(false);
    }

    /*public IEnumerator ScanGraphs () {
        foreach (var progress in AstarPath.active.ScanAsync()) {
            yield return null;
        }
    }*/

    private void Update()
    {
        if (!doOnce)
        {
            doOnce = true;

            ActualisePath();
        }
        
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(Death());
        }*/
        
        if (!noControl)
        {
            MovementsChara.Instance.RotateCharacter();

            if (controls.Character.Dash.WasPerformedThisFrame() && !isDashing)
            {
                DashChara.Instance.Dash();

                timerDash = DashChara.Instance.noHitTime;
                isDashing = true;
                
                anim.SetTrigger("isDashing");
            }
            
            if (controls.Character.Kick.WasPerformedThisFrame() && !isKicking)
            {
                if(activeGun != null) 
                    activeGun.GetComponent<SpriteRenderer>().enabled = false;
                
                anim.SetTrigger("isKicking");
                
                Vector2 mousePos = ReferenceCamera.Instance._camera.ScreenToWorldPoint(ManagerChara.Instance.controls.Character.MousePosition.ReadValue<Vector2>());
                Vector2 charaPos = ManagerChara.Instance.transform.position;

                Vector2 kickDirection;
                
                if (!keyboardDirection)
                {
                    kickDirection = new Vector2(-mousePos.x + charaPos.x, -mousePos.y + charaPos.y).normalized;
                }
                else
                {
                    kickDirection = -MovementsChara.Instance.direction.normalized;
                }
                
                StartCoroutine(KickChara.Instance.Kick(kickDirection));
            }

            if (isDashing)
            {
                timerDash -= Time.deltaTime;

                /*gameObject.layer = DashChara.Instance.invincibleLayer;
                Debug.Log(gameObject.layer);*/

                if (timerDash <= 0)
                {
                    isDashing = false;
                    doOnce = false;
                }
            }

            /*else
            {
                gameObject.layer = DashChara.Instance.normalLayer;
            }*/
            
            // SWITCH WEAPONS
            if (controls.Character.SwitchWeapon.ReadValue<float>() != 0 && canSwitch)
            {
                SwitchWeapons();
            }
        }

        else
        {
            if(!ReferenceCamera.Instance.finalCinematicChara)
                anim.SetBool("isWalking", false);
        }
    }

    
    public void ActualisePath()
    {
        var guo = new GraphUpdateObject(test.GetComponent<BoxCollider2D>().bounds);
        // Set some settings
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo);
    }


    private void FixedUpdate()
    {
        if(!isDashing && !noControl)
            MovementsChara.Instance.MoveCharacter();
    }
    

    void SwitchWeapons()
    {
        if (stockWeapon != null)
        {
            GameObject save = activeGun;

            HUDManager.Instance.ammo.DOColor(Color.white, 0);

            activeGun = stockWeapon;
            stockWeapon = save;

            activeGun.GetComponent<Gun>().isStocked = false;
            stockWeapon.GetComponent<Gun>().isStocked = true;
            
            stockWeapon.GetComponent<Gun>().Stocking();

            StartCoroutine(CooldownSwitch());

            HUDManager.Instance.UpdateAmmo(activeGun.GetComponent<Gun>().currentChargeurAmmo, activeGun.GetComponent<Gun>().gunData.chargeurSize, 
                activeGun.GetComponent<SpriteRenderer>().sprite);
            
            HUDManager.Instance.ReplaceImage(activeGun.GetComponent<Gun>().gunData.chargeurSize);
        }
    }

    private IEnumerator CooldownSwitch()
    {
        canSwitch = false;
        
        yield return new WaitForSeconds(switchCooldown);

        canSwitch = true;
    }

    public IEnumerator Death()
    {
        ScoreManager.instance.ActualiseScores();
        MenuMortManager.Instance.scoretext.text = "Score : " + ScoreManager.instance.scoreActuel;
        
        MenuMortManager.Instance.gameObject.SetActive(true);
        
        noControl = true;
        
        anim.SetTrigger("isDying");
        
        death.Play();
        
        ReferenceCamera.Instance.finalCinematicChara = true;

        MenuMortManager.Instance.fondMort.DOFade(1, 1);
        MenuMortManager.Instance.transform.DOMoveX(transform.position.x - 6.5f, 3).SetEase(Ease.OutSine);
        
        yield return new WaitForSeconds(1f);

        //Spot
        spotSprite.SetActive(true);
        spot.Play();
        
        Viseur.Instance.viseurActif = false;

        Viseur.Instance.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        CameraMovements.Instance.playerDeath = true;
        CameraMovements.Instance.timeZoom = 2;
        CameraMovements.Instance.posCamera = transform.position + new Vector3(-7, 0, 0);

        yield return new WaitForSeconds(2f);
    }


    public void Reset()
    {
        noControl = false;
        anim.SetTrigger("reset");

        LevelManager.Instance.activeGun = null;
        LevelManager.Instance.stockedGun = null;
    }
    

    public IEnumerator Fall(float fallDuration, Vector2 newPos)
    {
        fall.Play();
        transform.DOScale(Vector3.zero, fallDuration);
        noControl = true;
        isFalling = true;
        
        yield return new WaitForSeconds(fallDuration);

        transform.DOScale(Vector3.one, fallDuration / 5);
        noControl = false;
        isFalling = false;

        transform.position = newPos;
        
        HealthManager.Instance.LoseHealth(Vector2.zero);
    }


    public void StopChoroutines()
    {
        StopAllCoroutines();
        
        rb.velocity = Vector2.zero;
        isDashing = false;
        isKicking = false;
    }
}
