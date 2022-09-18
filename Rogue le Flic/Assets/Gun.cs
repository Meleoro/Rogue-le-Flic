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

public class Gun : MonoBehaviour
{
    [Header("References")]
    public GameObject bullet;
    private Controls controls;

    [Header("Shot Proprieties")] 
    public int nrbBulletsShot;
    public float cooldownShot;
    public float shotDispersion;

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
    }


    private void Update()
    {
        // ON POSITIONNE LE GUN SI LE JOUEUR LE PORTE
        if (!onGround)
        {
            transform.position = ManagerChara.Instance.transform.position;

            Vector2 mousePos = OrientateGun().normalized;
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
        if (controls.Character.Tir.IsPressed())
        {
            Shoot();
        }

        if (controls.Character.Enter.WasPerformedThisFrame())
        {
            if (canBePicked)
            {
                onGround = false;
            }
        }
    }


    public void Shoot()
    {
        if (!onGround && !onCooldown)
        {
            for (int k = 0; k < nrbBulletsShot; k++)
            {
                float dispersion = Random.Range(-shotDispersion, shotDispersion);
                
                Vector2 mousePos = OrientateGun();
                Vector2 gunPos = ReferenceCamera.Instance.camera.ScreenToWorldPoint(transform.position);
                
                float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

                GameObject refBullet = Instantiate(bullet, transform.position, 
                    Quaternion.AngleAxis(angle + dispersion, Vector3.forward));

                Destroy(refBullet, 4f);
            }
            
            onCooldown = true;

            StartCoroutine(ShotCooldown());
        }
    }

    public Vector2 OrientateGun()
    {
        return ReferenceCamera.Instance.camera.ScreenToWorldPoint(controls.Character.MousePosition.ReadValue<Vector2>());
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
