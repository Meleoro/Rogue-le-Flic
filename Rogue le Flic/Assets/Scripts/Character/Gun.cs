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
using Unity.VisualScripting.FullSerializer;
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

    [Header("ChargementTir")] 
    private float timerCharge;

    [Header("Effets Modules")]
    [HideInInspector] public bool ballesPercantes;
    [HideInInspector] public bool ballesRebondissantes;
    [HideInInspector] public bool grossissementBalles;
    [HideInInspector] public bool critiques;

    [Header("Shop")] 
    public string itemName;
    public string itemDescription;
    
    [Header("Others")]
    private float timerShot;
    private float timerLight;
    private float timerReload;
    [HideInInspector] public int currentChargeurAmmo;
    [HideInInspector] public int currentAmmo;

    [HideInInspector] public bool isStocked;
    [HideInInspector] public bool isHeld;
    [HideInInspector] public bool canBePicked;
    private bool onCooldown;
    private bool lookLeft;
    private bool isReloading;
    private bool stopStock;
    private float bowState;

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
        lightShot.intensity = 0;

        currentChargeurAmmo = gunData.chargeurSize;
        currentAmmo = gunData.maxAmmo;
    }


    private void Update()
    {
        if (!isStocked)
        {
            // ON POSITIONNE LE GUN SI LE JOUEUR LE PORTE
            if (isHeld)
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
                        
                        MovementsChara.Instance.lookLeft = true;
                    }
                    else
                    {
                        transform.rotation = Quaternion.AngleAxis(angle + addValue * 20, Vector3.forward);
                        
                        MovementsChara.Instance.lookLeft = false;
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

                    MovementsChara.Instance.lookLeft = true;
                }
                else
                {
                    transform.position = ManagerChara.Instance.transform.position + (Vector3) posRight;
                    
                    MovementsChara.Instance.lookLeft = false;
                }
                
                
            }

            else
            {
                transform.rotation = Quaternion.Euler(Vector3.zero);
            }
        
            // TIR
            if (controls.Character.Tir.IsPressed() && isHeld && !ManagerChara.Instance.noControl)
            {
                if ((!ManagerChara.Instance.munitionsActives || currentAmmo > 0) && !isReloading)
                {
                    if (gunData.tirChargeable)
                    {
                        if (timerCharge < gunData.dureeChargement)
                        {
                            timerCharge += Time.deltaTime;
                        }

                        if (timerCharge < gunData.dureeChargement / 3)
                        {
                            bowState = 1;
                            GetComponent<SpriteRenderer>().sprite = gunData.charge1;
                        }

                        else if (timerCharge < (gunData.dureeChargement / 3) * 2)
                        {
                            bowState = 2;
                            GetComponent<SpriteRenderer>().sprite = gunData.charge2;
                        }

                        else if (timerCharge < (gunData.dureeChargement / 3) * 3)
                        {
                            bowState = 3;
                            GetComponent<SpriteRenderer>().sprite = gunData.charge3;
                        }
                    
                        else
                        {
                            bowState = 4;
                            GetComponent<SpriteRenderer>().sprite = gunData.charge4;
                        }
                    }
                    else
                    {
                        Shoot();
                    }
                }
            }
            
            else if (controls.Character.Tir.WasReleasedThisFrame() && (!ManagerChara.Instance.munitionsActives || currentAmmo > 0) && 
                     gunData.tirChargeable && isHeld && !ManagerChara.Instance.noControl)
            {
                GetComponent<SpriteRenderer>().sprite = gunData.charge0;
                
                Shoot();
                
                timerCharge = 0;
            }
            

            // ON RAMASSE L'ARME
            if (controls.Character.Enter.WasPerformedThisFrame())
            {
                if (canBePicked && !isHeld)
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

            if (controls.Character.Reload.WasPerformedThisFrame() && !isReloading)
            {
                isReloading = true;
                timerReload = gunData.reloadTime;
            }
        }

        if (isReloading && isHeld)
        {
            ManagerChara.Instance.reload.GetComponentInParent<Canvas>().enabled = true;
            
            timerReload -= Time.deltaTime;

            ManagerChara.Instance.reload.fillAmount = 1 - timerReload / gunData.reloadTime;

            if (timerReload <= 0)
            {
                isReloading = false;
                currentChargeurAmmo = gunData.chargeurSize;

                ManagerChara.Instance.reload.GetComponentInParent<Canvas>().enabled = false;
                
                if(!ManagerChara.Instance.munitionsActives && !isStocked)
                {
                    HUDManager.Instance.UpdateAmmo(currentChargeurAmmo, gunData.chargeurSize, GetComponent<SpriteRenderer>().sprite);
                }
            }
        }
    }


    public void PickWeapon()
    {
        if (canBePicked)
        {
            isHeld = true;
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

            else if (ManagerChara.Instance.stockWeapon != null)
            {
                ManagerChara.Instance.activeGun.GetComponent<Gun>().isHeld = false;
                ManagerChara.Instance.activeGun.GetComponent<Gun>().canBePicked = false;
                
                ManagerChara.Instance.activeGun = gameObject;
            }
                        
            
            HUDManager.Instance.TakeWeapon();
            
            if (ManagerChara.Instance.munitionsActives && !isStocked)
            {
                HUDManager.Instance.UpdateAmmo(currentAmmo, gunData.maxAmmo, GetComponent<SpriteRenderer>().sprite);
            }
            else if(!isStocked)
            {
                HUDManager.Instance.UpdateAmmo(currentChargeurAmmo, gunData.chargeurSize, GetComponent<SpriteRenderer>().sprite);
            }
        }
    }
    
    public void Shoot()
    {
        if (!onCooldown && !isReloading && isHeld)
        {
            // BOUCLE QUI GENERE TOUTES LES BALLES
            for (int k = 0; k < gunData.nbrBulletPerShot; k++)
            {
                float dispersion = Random.Range(-gunData.shotDispersion, gunData.shotDispersion);

                float angle;

                angle = OrientateGun();
                
                GameObject refBullet = Instantiate(bullet, ManagerChara.Instance.transform.position, 
                    Quaternion.AngleAxis(angle + dispersion, Vector3.forward));

                
                if (refBullet.GetComponent<Bullet>().isBubble)
                {
                    refBullet.GetComponent<Bullet>().originalVelocity = ManagerChara.Instance.rb.velocity;
                }
                else if (refBullet.GetComponent<Bullet>().isArrow)
                {
                    refBullet.GetComponent<Bullet>().timerArrow = bowState / 3;

                    refBullet.GetComponent<Bullet>().bulletSpeed = bowState * 10;
                }

                
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

                if (critiques)
                {
                    int isCritique = Random.Range(0, 100);

                    if (isCritique <= ModuleManager.Instance.probaCritique)
                    {
                        refBullet.GetComponent<Bullet>().isCritique = true;
                    }
                }
                
                if(!refBullet.GetComponent<Bullet>().isArrow)
                    Destroy(refBullet, 3f);
            }

            currentChargeurAmmo -= 1;
            currentAmmo -= 1;

            if (currentChargeurAmmo <= 0)
            {
                isReloading = true;
                timerReload = gunData.reloadTime;
            }
            
            onCooldown = true;
            timerLight = gunData.lightShotDuration;
            timerShot = 1;
            
            StartCoroutine(ShotCooldown());
            Knockback();

            if(CameraMovements.Instance.canShake)
                ReferenceCamera.Instance.transform.DOShakePosition(gunData.shakeDuration, gunData.shakeAmplitude);

            if (ManagerChara.Instance.munitionsActives)
            {
                HUDManager.Instance.UpdateAmmo(currentAmmo, gunData.maxAmmo, GetComponent<SpriteRenderer>().sprite);
            }
            else
            {
                HUDManager.Instance.UpdateAmmo(currentChargeurAmmo, gunData.chargeurSize, GetComponent<SpriteRenderer>().sprite);
            }
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

        /*if (currentAmmo < gunData.maxAmmo)
        {
            StartCoroutine(ReloadCooldown());
        }*/
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
    

    public void AddAmmo(int ammoAdded)
    {
        currentAmmo += ammoAdded;

        if (currentAmmo > gunData.maxAmmo)
        {
            currentAmmo = gunData.maxAmmo;
        }
        
        if (ManagerChara.Instance.munitionsActives)
        {
            HUDManager.Instance.UpdateAmmo(currentAmmo, gunData.maxAmmo, GetComponent<SpriteRenderer>().sprite);
        }
    }
}
