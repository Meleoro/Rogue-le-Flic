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

    private Gun gun;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if(ManagerChara.Instance.activeGun != null)
            gun = ManagerChara.Instance.activeGun.GetComponent<Gun>();
        
        if (Module1 != 0)
        {
            Search(Module1);
        }
        
        if (Module2 != 0)
        {
            Search(Module2);
        }
    }

    private void LateUpdate()
    {
        /*if (gun is not null)
        {
            gun.bulletSize = gun.originalBulletSize;
            gun.doubleBullet = false;
        }*/
    }


    public void Search(int module)
    {
        if(module == 1)
            Effet1();
            
        else if(module == 2)
            Effet2();
    }
    

    // GROSSISSEMENT DES BALLES
    public void Effet1()
    {
        gun.bulletSize = gun.originalBulletSize * 3;
    }
    
    // DOUBLE TIRE
    public void Effet2()
    {
        gun.doubleBullet = true;
    }
}
