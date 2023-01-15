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
    public int levelModule1;
    public int levelModule2;

    [Header("Infos1")] 
    public string moduleName1;
    public string description1;
    public Sprite image1;
    
    [Header("Infos2")]
    public string moduleName2;
    public string description2;
    public Sprite image2;
    
    
    [Header("Perçantes")] 
    public int nbrPercagesMaxLvl1;
    public int nbrPercagesMaxLvl2;
    public int nbrPercagesMaxLvl3;
    [HideInInspector] public int nbrPercagesMax;
    
    [Header("Rebonds")] 
    public int nbrRebondsMaxLvl1;
    public int nbrRebondsMaxLvl2;
    public int nbrRebondsMaxLvl3;
    [HideInInspector] public int nbrRebondsMax;

    [Header("Grossissement")] 
    public float multiplicateurTailleLvl1;
    public float multiplicateurTailleLvl2;
    public float multiplicateurTailleLvl3;
    [HideInInspector] public float multiplicateurTaille;

    [Header("Critique")] 
    [Range(0, 100)] public float probaCritiqueLvl1;
    [Range(0, 100)] public float probaCritiqueLvl2;
    [Range(0, 100)] public float probaCritiqueLvl3;
    [HideInInspector] public float probaCritique;
    public float multiplicateurTailleCrit;
    public float multiplicateurVitesseCrit;
    public float multiplicateurDegatsCrit;
    
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
            Search(Module1, 1);
        }
        
        if (Module2 != 0)
        {
            Search(Module2, 2);
        }
    }


    public void Search(int module, int nbrEmplacement)
    {
        if (module == 1)
            Effet1(nbrEmplacement);

        else if (module == 2)
            Effet2(nbrEmplacement);

        else if (module == 3)
            Effet3(nbrEmplacement);
        
        else if (module == 4)
            Effet4(nbrEmplacement);
    }
    
    

    // BALLES PERCANTES
    public void Effet1(int nbrEmplacement)
    {
        gun.ballesPercantes = true;
    }
    
    
    // BALLES REBONDISSANTES
    public void Effet2(int nbrEmplacement)
    {
        gun.ballesRebondissantes = true;

        if (nbrEmplacement == 1)
        {
            if (levelModule1 == 1)
            {
                nbrRebondsMax = nbrRebondsMaxLvl1;
            }
            else if (levelModule1 == 2)
            {
                nbrRebondsMax = nbrRebondsMaxLvl2;
            }
            else
            {
                nbrRebondsMax = nbrRebondsMaxLvl3;
            }
        }

        else
        {
            if (levelModule2 == 1)
            {
                nbrRebondsMax = nbrRebondsMaxLvl1;
            }
            else if (levelModule2 == 2)
            {
                nbrRebondsMax = nbrRebondsMaxLvl2;
            }
            else
            {
                nbrRebondsMax = nbrRebondsMaxLvl3;
            }
        }
    }
    

    public void Effet3(int nbrEmplacement)
    {
        gun.grossissementBalles = true;
        
        if (levelModule1 == 1)
        {
            multiplicateurTaille = multiplicateurTailleLvl1;
        }
        else if (levelModule1 == 2)
        {
            multiplicateurTaille = multiplicateurTailleLvl2;
        }
        else
        {
            multiplicateurTaille = multiplicateurTailleLvl3;
        }
    }
    
    
    public void Effet4(int nbrEmplacement)
    {
        gun.critiques = true;
        
        if (levelModule1 == 1)
        {
            probaCritique = probaCritiqueLvl1;
        }
        else if (levelModule1 == 2)
        {
            probaCritique = probaCritiqueLvl2;
        }
        else
        {
            probaCritique = probaCritiqueLvl3;
        }
    }
}