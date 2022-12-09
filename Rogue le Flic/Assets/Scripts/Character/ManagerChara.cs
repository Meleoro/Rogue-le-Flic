using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerChara : MonoBehaviour
{
    public static ManagerChara Instance;
    
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public GameObject activeGun;
    [HideInInspector] public GameObject stockWeapon;

    public Controls controls;
    [HideInInspector] public bool noControl;
    public bool munitionsActives;

    [Header("Dash")] 
    [HideInInspector] public bool isDashing;
    private float timerDash;
    
    [Header("Kick")] 
    private bool isKicking;
    private float timerKick;

    [Header("Repositionnement")] 
    public Vector2 posLeft;
    public Vector2 posRight;

    [Header("Autres")] 
    public float switchCooldown;
    public bool canSwitch;
    public Animator anim;
    public Image reload;
    [HideInInspector] public bool isFalling;
    
    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);

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
    }

    private void Update()
    {
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
                activeGun.GetComponent<SpriteRenderer>().enabled = false;
                
                anim.SetTrigger("isKicking");
                StartCoroutine(KickChara.Instance.Kick());
            }

            if (isDashing)
            {
                timerDash -= Time.deltaTime;

                /*gameObject.layer = DashChara.Instance.invincibleLayer;
                Debug.Log(gameObject.layer);*/

                if (timerDash <= 0)
                {
                    isDashing = false;
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

            activeGun = stockWeapon;
            stockWeapon = save;

            activeGun.GetComponent<Gun>().isStocked = false;
            stockWeapon.GetComponent<Gun>().isStocked = true;
            
            stockWeapon.GetComponent<Gun>().Stocking();

            StartCoroutine(CooldownSwitch());
            
            if (munitionsActives)
            {
                HUDManager.Instance.UpdateAmmo(activeGun.GetComponent<Gun>().currentAmmo, activeGun.GetComponent<Gun>().gunData.maxAmmo);
            }
            else
            {
                HUDManager.Instance.UpdateAmmo(activeGun.GetComponent<Gun>().currentChargeurAmmo, activeGun.GetComponent<Gun>().gunData.chargeurSize);
            }
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
        anim.SetTrigger("isDying");

        yield return new WaitForSeconds(0.9f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator Fall(float fallDuration, Vector2 newPos)
    {
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
}
