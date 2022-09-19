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

    [Header("Others")] 
    public float rotationSpeed;
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

        lightShot.intensity = 0;
    }


    private void Update()
    {
        // ON POSITIONNE LE GUN SI LE JOUEUR LE PORTE
        if (!onGround)
        {
            transform.position = ManagerChara.Instance.transform.position;
            
            float angle = OrientateGun();
            
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
            }
        }

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

                Destroy(refBullet, 3f);
            }
            
            onCooldown = true;
            timerLight = lightShotDuration;
            StartCoroutine(ShotCooldown());
            
            ReferenceCamera.Instance.camera.DOShakePosition(shakeDuration, shakeAmplitude);
        }
    }
    
    
    public float OrientateGun()
    {
        Vector2 mousePos = ReferenceCamera.Instance.camera.ScreenToViewportPoint(controls.Character.MousePosition.ReadValue<Vector2>());
        Vector2 charaPos = ReferenceCamera.Instance.camera.WorldToViewportPoint(ManagerChara.Instance.transform.position);
        
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
}
