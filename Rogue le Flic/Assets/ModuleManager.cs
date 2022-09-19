using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ModuleManager : MonoBehaviour
{
    public static ModuleManager Instance;
    public int Module1;
    public int Module2;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Module1 != 0)
        {
            if(Module1 == 1)
                Effet1();
            
            else if(Module1 == 2)
                Effet2();
        }


        if (Module2 != 0)
        {
            if(Module2 == 1)
                Effet1();
            
            else if(Module2 == 2)
                Effet2();
        }
    }


    // GROSSISSEMENT DES BALLES
    public void Effet1()
    {
        Gun gun = ManagerChara.Instance.activeGun.GetComponent<Gun>();

        gun.bulletSize = gun.originalBulletSize * 3;
    }
    
    // DOUBLE TIRE
    public void Effet2()
    {
        Gun gun = ManagerChara.Instance.activeGun.GetComponent<Gun>();

        gun.doubleBullet = true;
    }
}
