using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using DG.Tweening;
using UnityEngine.Rendering.Universal;


public class Gun : MonoBehaviour
{
    [Header("References")]
    public GameObject bullet;
    public Light2D lightShot;
    private Controls controls;
    public GunData gunData;

    [Header("Positions")] 
    public Vector2 posLeft;
    public Vector2 posRight;

    [Header("Effets Modules")]
    [HideInInspector] public bool ballesPercantes;
    [HideInInspector] public bool ballesRebondissantes;
    [HideInInspector] public bool grossissementBalles;
    
    [Header("Others")]
    private float timerShot;
    private float timerLight;
    private int currentAmmo;

    [HideInInspector] public bool isStocked;
    private bool onGround;
    private bool canBePicked;
    private bool onCooldown;
    private bool lookLeft;
    private bool isReloading;
    private bool stopStock;
    [HideInInspector] public bool autoAim;

    public GameObject explanation;


    private void Awake()
    {
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
        onGround = true;

        lightShot.intensity = 0;

        currentAmmo = gunData.maxAmmo;
    }


    private void Update()
    {
        if (!isStocked)
        {
            // ON POSITIONNE LE GUN SI LE JOUEUR LE PORTE
            if (!onGround)
            {
                // ON RETIRE LE TEXTE
                explanation.SetActive(false);

                float angle = OrientateGun();
            
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                if (timerShot > 0)
                {
                    timerShot -= Time.deltaTime;

                    float addValue = gunData.gunKnockback.Evaluate(1 - timerShot);

                    if (angle > 90 || angle < -90)
                    {
                        transform.rotation = Quaternion.AngleAxis(angle - addValue * 20, Vector3.forward);
                    }
                    else
                    {
                        transform.rotation = Quaternion.AngleAxis(angle + addValue * 20, Vector3.forward);
                    }
                }
                else
                {
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            
                if (angle > 90 || angle < -90)
                {
                    transform.position = ManagerChara.Instance.transform.position + (Vector3) posLeft;
                    transform.Rotate(180, 0, 0);
                }
                else
                {
                    transform.position = ManagerChara.Instance.transform.position + (Vector3) posRight;
                }
            }
        
            // TIR
            if (controls.Character.Tir.IsPressed())
            {
                Shoot();
            }

            // ON RAMASSE L'ARME
            if (controls.Character.Enter.WasPerformedThisFrame())
            {
                if (canBePicked && (ManagerChara.Instance.stockWeapon == null || ManagerChara.Instance.activeGun == null))
                {
                    PickWeapon();
                }
            }


            // GESTION DE LA LUMIERE QUAND LE JOUEUR TIRE
            if (timerLight > 0)
            {
                timerLight -= Time.deltaTime;
                lightShot.intensity = timerLight * gunData.lightShotIntensity;
            }
            else
                lightShot.intensity = 0;
        }
    }


    public void PickWeapon()
    {
        if (canBePicked)
        {
            onGround = false;
            canBePicked = false;

            //explanation.SetActive(false);
            
            if (ManagerChara.Instance.activeGun == null)
            {
                ManagerChara.Instance.activeGun = gameObject;
            }
            
            else if (ManagerChara.Instance.stockWeapon == null)
            {
                ManagerChara.Instance.stockWeapon = gameObject;
                
                isStocked = true;
                
                Stocking();
            }

            else if(ManagerChara.Instance.stockWeapon != null)
            {
                ManagerChara.Instance.activeGun.GetComponent<Gun>().onGround = true;
                ManagerChara.Instance.activeGun.GetComponent<Gun>().canBePicked = true;
                
                ManagerChara.Instance.activeGun = gameObject;
            }
        }
    }
    
    public void Shoot()
    {
        if (!onGround && !onCooldown && !isReloading)
        {
            // BOUCLE QUI GENERE TOUTES LES BALLES
            for (int k = 0; k < gunData.nbrBulletPerShot; k++)
            {
                float dispersion = Random.Range(-gunData.shotDispersion, gunData.shotDispersion);

                float angle;

                if (autoAim)
                {
                    Vector2 ennemyPos = KickChara.Instance.kickedEnnemy.transform.position;
                    Vector2 charaPos = ManagerChara.Instance.transform.position;
        
                    angle = Mathf.Atan2(ennemyPos.y - charaPos.y, ennemyPos.x - charaPos.x) * Mathf.Rad2Deg;
                }
                else
                {
                    angle = OrientateGun();
                }

                GameObject refBullet = Instantiate(bullet, ManagerChara.Instance.transform.position, 
                    Quaternion.AngleAxis(angle + dispersion, Vector3.forward));

                refBullet.GetComponent<Bullet>().bulletDamages = gunData.damages;

                // EFFETS MODULES
                if (ballesPercantes)
                {
                    refBullet.GetComponent<Bullet>().percante = true;
                }

                if (ballesRebondissantes)
                {
                    refBullet.GetComponent<Bullet>().rebondissante = true;
                }

                if (grossissementBalles)
                {
                    refBullet.GetComponent<CircleCollider2D>().radius = (float) 0.15 * ModuleManager.Instance.multiplicateurTaille;
                    refBullet.GetComponent<Bullet>().objetAGrossir.transform.localScale = new Vector3(1 * ModuleManager.Instance.multiplicateurTaille, 
                        1 * ModuleManager.Instance.multiplicateurTaille, 1);
                }
                
                Destroy(refBullet, 3f);
            }

            currentAmmo -= 1;

            if (currentAmmo <= 0)
            {
                isReloading = true;
                StartCoroutine(ReloadCooldown());
            }
            
            onCooldown = true;
            timerLight = gunData.lightShotDuration;
            timerShot = 1;
            
            StartCoroutine(ShotCooldown());
            Knockback();

            ReferenceCamera.Instance.transform.DOShakePosition(gunData.shakeDuration, gunData.shakeAmplitude);
        }
    }

    public void Knockback()
    {
        Vector2 mousePos = ReferenceCamera.Instance._camera.ScreenToViewportPoint(controls.Character.MousePosition.ReadValue<Vector2>());
        Vector2 charaPos = ReferenceCamera.Instance._camera.WorldToViewportPoint(ManagerChara.Instance.transform.position);

        Vector2 direction = charaPos - mousePos;

        ManagerChara.Instance.rb.AddForce(direction.normalized * gunData.charaKnockback, ForceMode2D.Impulse);
    }
    
    public float OrientateGun()
    {
        Vector2 mousePos = ReferenceCamera.Instance._camera.ScreenToWorldPoint(controls.Character.MousePosition.ReadValue<Vector2>());
        Vector2 charaPos = ManagerChara.Instance.transform.position;
        
        return Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
    }

    public void Stocking()
    {
        transform.position = new Vector2(1000, 1000);

        onCooldown = false;

        StopAllCoroutines();

        if (currentAmmo < gunData.maxAmmo)
        {
            StartCoroutine(ReloadCooldown());
        }
    }

    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            canBePicked = true;
            explanation.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canBePicked = false;
            explanation.SetActive(false);
        }
    }

    
    IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(gunData.cooldownShot);

        onCooldown = false;
    }
    
    IEnumerator ReloadCooldown()
    {
        yield return new WaitForSeconds(gunData.reloadTime);

        isReloading = false;
        currentAmmo = gunData.maxAmmo;
    }

    public IEnumerator AutoAim(float duree)
    {
        autoAim = true;
        
        yield return new WaitForSeconds(duree);
        
        autoAim = false;
    }
}
