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
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine.Rendering.Universal;


public class Gun : MonoBehaviour
{
    [Header("References")]
    public GameObject bullet;
    public Light2D lightShot;
    private Controls controls;
    public GunData gunData;
    public GameObject VFXTake;

    [Header("UI")] 
    public GameObject UIArme;
    public TextMeshProUGUI textNom;

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
    [HideInInspector] public bool isBoosted;
    private int chargeurSize;
    private float currentFireRate;
    private float currentdureeChargement;

    [Header("Colors")]
    private bool isRed;
    private bool isWhite;

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

    public float trajetArrow;

    public GameObject explanation;

    public bool isDontDestoy;
    public int weaponType;

    private Sprite spriteWeapon;

    public AudioSource pickup;

    public AudioSource shoot;

    public AudioSource reload;

    public AudioSource dryshot;
    
    
    
    public bool isArc = false;
    //public AudioSource shoot;
    


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

        VFXTake.SetActive(true);

        chargeurSize = gunData.chargeurSize;
        currentFireRate = gunData.cooldownShot;
        currentdureeChargement = gunData.dureeChargement;

        currentChargeurAmmo = chargeurSize;

        spriteWeapon = GetComponent<SpriteRenderer>().sprite;
    }


    private void Update()
    {
        if (!isStocked)
        {
            VFXTake.SetActive(true);
            
            // ON POSITIONNE LE GUN SI LE JOUEUR LE PORTE
            if (isHeld)
            {
                VFXTake.SetActive(false);
                
                // ON RETIRE LE TEXTE
                explanation.SetActive(false);
                UIArme.SetActive(false);

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
                
                
                //MODULE BOOST
                if (isBoosted)
                {
                    chargeurSize = (int) (gunData.chargeurSize * ModuleManager.Instance.multiplicateurChargeur);
                    currentFireRate = gunData.cooldownShot / ModuleManager.Instance.multiplicateurFireRate;
                    currentdureeChargement = gunData.dureeChargement / ModuleManager.Instance.multiplicateurFireRate;
                }
                else
                {
                    chargeurSize = gunData.chargeurSize;
                    currentFireRate = gunData.cooldownShot;
                    currentdureeChargement = gunData.dureeChargement;
                }
                
                HUDManager.Instance.UpdateAmmo(currentChargeurAmmo, chargeurSize, spriteWeapon);
                
                
                // MANQUE DE MUNITIONS
                if (currentChargeurAmmo < (int)(chargeurSize * 0.3f))
                {
                    if (!isRed)
                    {
                        isRed = true;
                        isWhite = true;

                        HUDManager.Instance.ammo.DOColor(Color.red, 0.1f).OnComplete((() => isWhite = false));
                    }
                    
                    else if (!isWhite)
                    {
                        isRed = true;
                        isWhite = true;

                        HUDManager.Instance.ammo.DOColor(Color.white, 0.3f).OnComplete((() => isRed = false));
                    }
                }
                else
                {
                    isRed = false;
                    
                    HUDManager.Instance.ammo.DOColor(Color.white, 0);
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
                        if (timerCharge < currentdureeChargement)
                        {
                            timerCharge += Time.deltaTime;
                        }

                        if (timerCharge < currentdureeChargement / 3)
                        {
                            bowState = 1;
                            GetComponent<SpriteRenderer>().sprite = gunData.charge1;
                        }

                        else if (timerCharge < (currentdureeChargement / 3) * 2)
                        {
                            bowState = 2;
                            GetComponent<SpriteRenderer>().sprite = gunData.charge2;
                        }

                        else if (timerCharge < (currentdureeChargement / 3) * 3)
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

            if (controls.Character.Reload.WasPerformedThisFrame() && !isReloading && currentChargeurAmmo != chargeurSize)
            {
                isReloading = true;
                timerReload = gunData.reloadTime;
                
                //here2
                reload.Play();

                
            }
        }

        if (isReloading && isHeld)
        {
            VFXTake.SetActive(false);
            
            ManagerChara.Instance.reload.GetComponentInParent<Canvas>().enabled = true;
            
            timerReload -= Time.deltaTime;

            ManagerChara.Instance.reload.fillAmount = 1 - timerReload / gunData.reloadTime;

            if (timerReload <= 0)
            {
                isReloading = false;
                currentChargeurAmmo = chargeurSize;

                ManagerChara.Instance.reload.GetComponentInParent<Canvas>().enabled = false;
                
                if(!ManagerChara.Instance.munitionsActives && !isStocked)
                {
                    HUDManager.Instance.UpdateAmmo(currentChargeurAmmo, chargeurSize, spriteWeapon);
                }
            }
        }
    }


    public void PickWeapon()
    {
        if (canBePicked)
        {
            pickup.Play();

            if (ModuleManager.Instance.Module1 == 5 || ModuleManager.Instance.Module1 == 5)
            {
                chargeurSize = (int)(gunData.chargeurSize * ModuleManager.Instance.multiplicateurChargeur);
                currentFireRate = gunData.cooldownShot / ModuleManager.Instance.multiplicateurFireRate;
                currentdureeChargement = gunData.dureeChargement / ModuleManager.Instance.multiplicateurFireRate;
            }

            currentChargeurAmmo = chargeurSize;

            isHeld = true;
            canBePicked = false;

            //explanation.SetActive(false);
            
            UIArme.SetActive(false);

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

            /*if (ManagerChara.Instance.munitionsActives && !isStocked)
            {
                HUDManager.Instance.UpdateAmmo(currentAmmo, gunData.maxAmmo, GetComponent<SpriteRenderer>().sprite);
            }*/

            if (!isStocked)
            {
                HUDManager.Instance.UpdateAmmo(currentChargeurAmmo, chargeurSize, GetComponent<SpriteRenderer>().sprite);
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


                //here
                //gunData.shoot.Play();
                
                shoot.Play();
                
                    GameObject refBullet = Instantiate(bullet, ManagerChara.Instance.transform.position, 
                    Quaternion.AngleAxis(angle + dispersion, Vector3.forward));

                
                if (refBullet.GetComponent<Bullet>().isBubble)
                {
                    refBullet.GetComponent<Bullet>().originalVelocity = ManagerChara.Instance.rb.velocity;
                }
                else if (refBullet.GetComponent<Bullet>().isArrow)
                {
                    refBullet.GetComponent<Bullet>().timerArrow = bowState / trajetArrow;

                    refBullet.GetComponent<Bullet>().bulletSpeed = bowState * 10;
                }

                
                refBullet.GetComponent<Bullet>().bulletDamages = gunData.damages;

                
                // EFFETS MODULES
                if (ballesPercantes)
                {
                    refBullet.GetComponent<Bullet>().percante = true;
                    refBullet.GetComponent<Bullet>().nbrPercesMax = ModuleManager.Instance.nbrPercagesMax;
                }

                if (ballesRebondissantes)
                {
                    refBullet.GetComponent<Bullet>().rebondissante = true;
                    refBullet.GetComponent<Bullet>().nbrRebondsMax = ModuleManager.Instance.nbrRebondsMax;
                }

                if (grossissementBalles)
                {
                    refBullet.GetComponent<CircleCollider2D>().radius = (float) 0.15 * ModuleManager.Instance.multiplicateurTaille;
                    if(!refBullet.GetComponent<Bullet>().isArrow)
                        refBullet.GetComponent<Bullet>().objetAGrossir.transform.localScale = new Vector3(1 * ModuleManager.Instance.multiplicateurTaille, 
                            1 * ModuleManager.Instance.multiplicateurTaille, 1);
                    
                    else
                        refBullet.transform.localScale = new Vector3(1 * ModuleManager.Instance.multiplicateurTaille, 
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

                if (isArc == false)
                {
                    dryshot.Play();
                
                    reload.Play();
                }


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
            
            HUDManager.Instance.UpdateAmmo(currentChargeurAmmo, chargeurSize, GetComponent<SpriteRenderer>().sprite);
            

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
            
            UIArme.SetActive(true);
            textNom.text = gunData.itemName;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canBePicked = false;
            explanation.SetActive(false);
            
            UIArme.SetActive(false);
        }
    }

    
    IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(currentFireRate);

        onCooldown = false;
        

    }
    

    /*public void AddAmmo(int ammoAdded)
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
    }*/
}
