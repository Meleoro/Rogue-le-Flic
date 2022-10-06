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
    [HideInInspector] public float bulletSize;
    [HideInInspector] public bool doubleBullet;

    [Header("Others")]
    private float timerShot;
    private float timerLight;
    private bool onGround;
    private bool canBePicked;
    private bool onCooldown;
    private bool lookLeft;


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

        bulletSize = gunData.originalBulletSize;
        lightShot.intensity = 0;
    }


    private void Update()
    {
        // ON POSITIONNE LE GUN SI LE JOUEUR LE PORTE
        if (!onGround)
        {
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
            if (canBePicked)
            {
                onGround = false;

                ManagerChara.Instance.activeGun = gameObject;
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


    public void Shoot()
    {
        if (!onGround && !onCooldown)
        {
            // BOUCLE QUI GENERE TOUTES LES BALLES
            for (int k = 0; k < gunData.nbrBulletPerShot; k++)
            {
                float dispersion = Random.Range(-gunData.shotDispersion, gunData.shotDispersion);

                float angle = OrientateGun();
                
                GameObject refBullet = Instantiate(bullet, transform.position, 
                    Quaternion.AngleAxis(angle + dispersion, Vector3.forward));
                
                
                // DOUBLE TIR ?
                if (doubleBullet)
                {
                    StartCoroutine(DoubleShot());
                }
                
                // TAILLE DES TIRS ?
                refBullet.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
                
                Destroy(refBullet, 3f);
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
        Vector2 mousePos = ReferenceCamera.Instance.camera.ScreenToViewportPoint(controls.Character.MousePosition.ReadValue<Vector2>());
        Vector2 charaPos = ReferenceCamera.Instance.camera.WorldToViewportPoint(ManagerChara.Instance.transform.position);

        Vector2 direction = charaPos - mousePos;

        ManagerChara.Instance.rb.AddForce(direction.normalized * gunData.charaKnockback, ForceMode2D.Impulse);
    }
    
    public float OrientateGun()
    {
        Vector2 mousePos = ReferenceCamera.Instance.camera.ScreenToWorldPoint(controls.Character.MousePosition.ReadValue<Vector2>());
        Vector2 charaPos = ManagerChara.Instance.transform.position;
        
        return Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
    }

    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
            canBePicked = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
            canBePicked = false;
    }

    IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(gunData.cooldownShot);

        onCooldown = false;
    }

    IEnumerator DoubleShot()
    {
        yield return new WaitForSeconds(0.05f);

        float dispersion = Random.Range(-gunData.shotDispersion, gunData.shotDispersion);
                
        float angle = OrientateGun();
                
        GameObject refBullet = Instantiate(bullet, transform.position, 
            Quaternion.AngleAxis(angle + dispersion, Vector3.forward));
        
        // TAILLE DES TIRS ?
        refBullet.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);

        Destroy(refBullet, 3f);
        
    }
}
