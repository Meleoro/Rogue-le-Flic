using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ModuleManager : MonoBehaviour
{
    public static ModuleManager Instance;
    [HideInInspector] public int Module1;
    [HideInInspector] public int Module2;

    [Header("Rebonds")] 
    [SerializeField] private int nbrRebondsMax;

    [Header("Grossissement")] 
    public float multiplicateurTaille;
    
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

        if (gun is not null)
        {
            gun.ballesPercantes = false;
            gun.ballesRebondissantes = false;
            gun.grossissementBalles = false;
        }
        
        if (Module1 != 0)
        {
            Search(Module1);
        }
        
        if (Module2 != 0)
        {
            Search(Module2);
        }
    }


    public void Search(int module)
    {
        if (module == 1)
            Effet1();

        else if (module == 2)
            Effet2();

        else if (module == 3)
            Effet3();
    }
    

    // GROSSISSEMENT DES BALLES
    public void Effet1()
    {
        gun.ballesPercantes = true;
    }
    
    // DOUBLE TIRE
    public void Effet2()
    {
        gun.ballesRebondissantes = true;
    }

    public void Effet3()
    {
        gun.grossissementBalles = true;
    }
}


