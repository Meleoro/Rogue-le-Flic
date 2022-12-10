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

    [Header("Infos1")] 
    public string moduleName1;
    public string description1;
    public Sprite image1;
    
    [Header("Infos2")]
    public string moduleName2;
    public string description2;
    public Sprite image2;
    
    [Header("Rebonds")] 
    //[SerializeField] private int nbrRebondsMax;

    [Header("Grossissement")] 
    public float multiplicateurTaille;

    [Header("Critique")] 
    [Range(0, 100)] public float probaCritique;
    public float mulitplicateurTaille;
    public float multiplicateurVitesse;
    public float multiplicateurDegats;
    
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
            gun.critiques = false;
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
        
        else if (module == 4)
            Effet4();
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

    public void Effet4()
    {
        gun.critiques = true;
    }
}