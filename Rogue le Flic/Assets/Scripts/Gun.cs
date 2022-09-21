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

    [Header("Positions")] 
    public Vector2 posLeft;
    public Vector2 posRight;

    [Header("Shot Proprieties")] 
    public int nrbBulletsShot;
    public float cooldownShot;
    public float shotDispersion;

    [Header("Camera Shake")] 
    public float shakeDuration;
    public float shakeAmplitude;

    [Header("Light")] 
    public float lightShotIntensity;
    public float lightShotDuration;
    private float timerLight;

    [Header("Effets Modules")] 
    public float originalBulletSize;
    [HideInInspector] public float bulletSize;
    public bool doubleBullet;

    [Header("Others")] 
    public float knockback;
    private bool onGround;
    private bool canBePicked;
    private bool onCooldown;


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

        bulletSize = originalBulletSize;
        lightShot.intensity = 0;
    }


    private void LateUpdate()
    {
        // ON POSITIONNE LE GUN SI LE JOUEUR LE PORTE
        if (!onGround)
        {
            float angle = OrientateGun();
            
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            
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
            lightShot.intensity = timerLight * lightShotIntensity;
        }
        else
            lightShot.intensity = 0;
    }


    public void Shoot()
    {
        if (!onGround && !onCooldown)
        {
            // BOUCLE QUI GENERE TOUTES LES BALLES
            for (int k = 0; k < nrbBulletsShot; k++)
            {
                float dispersion = Random.Range(-shotDispersion, shotDispersion);

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
            timerLight = lightShotDuration;
            
            StartCoroutine(ShotCooldown());
            Knockback();

            ReferenceCamera.Instance.camera.DOShakePosition(shakeDuration, shakeAmplitude);
        }
    }

    public void Knockback()
    {
        Vector2 mousePos = ReferenceCamera.Instance.camera.ScreenToViewportPoint(controls.Character.MousePosition.ReadValue<Vector2>());
        Vector2 charaPos = ReferenceCamera.Instance.camera.WorldToViewportPoint(ManagerChara.Instance.transform.position);

        Vector2 direction = charaPos - mousePos;

        ManagerChara.Instance.rb.AddForce(direction.normalized * knockback, ForceMode2D.Impulse);
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
        yield return new WaitForSeconds(cooldownShot);

        onCooldown = false;
    }

    IEnumerator DoubleShot()
    {
        yield return new WaitForSeconds(0.05f);

        float dispersion = Random.Range(-shotDispersion, shotDispersion);
                
        float angle = OrientateGun();
                
        GameObject refBullet = Instantiate(bullet, transform.position, 
            Quaternion.AngleAxis(angle + dispersion, Vector3.forward));
        
        // TAILLE DES TIRS ?
        refBullet.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);

        Destroy(refBullet, 3f);
        
    }
}
