using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

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
        if (!onGround)
        {
            transform.position = ManagerChara.Instance.transform.position;
            
            float angle = OrientateGun();
            
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
        if (controls.Character.Tir.WasPerformedThisFrame())
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

                float angle = OrientateGun();
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                GameObject refBullet = Instantiate(bullet, transform.position, 
                    Quaternion.EulerAngles(rotation.x, rotation.y, rotation.z + dispersion));

                Destroy(refBullet, 4f);
            }
            
            onCooldown = true;

            StartCoroutine(ShotCooldown());
        }
    }

    public float OrientateGun()
    {
        Vector2 mousePos = ReferenceCamera.Instance.camera.ScreenToWorldPoint(controls.Character.MousePosition.ReadValue<Vector2>());
        
        return Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
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
