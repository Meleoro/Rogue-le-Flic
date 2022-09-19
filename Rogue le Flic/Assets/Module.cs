using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    private bool canBeGrab;

    public int numberEffect;
    

    private void Update()
    {
        if (MovementsChara.Instance.controls.Character.Enter.WasPerformedThisFrame() && canBeGrab)
        {
            if (numberEffect == 1)
            {
                Effet1();
            }
            
            else if (numberEffect == 2)
            {
                Effet2();
            }

            Destroy(gameObject);
        }
        
    }


    // GROSSISSEMENT DES BALLES
    public void Effet1()
    {
        Gun gun = ManagerChara.Instance.activeGun.GetComponent<Gun>();

        gun.bulletSize *= 4;
    }
    
    // DOUBLE TIRE
    public void Effet2()
    {
        Gun gun = ManagerChara.Instance.activeGun.GetComponent<Gun>();

        gun.doubleBullet = true;
    }
    
    
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            canBeGrab = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            canBeGrab = false;
        }
    }
}
