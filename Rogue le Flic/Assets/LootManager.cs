using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance;
    
    [Header("Loot Weapons")]
    public List<GameObject> weaponsLevel1;
    public List<GameObject> weaponsLevel2;
    public List<GameObject> weaponsLevel3;
    
    [Header("Loot Modules")]
    public List<GameObject> modulesLevel1;
    public List<GameObject> modulesLevel2;
    public List<GameObject> modulesLevel3;

    [Header("Proba Drop Loot")]
    [Range(0, 100)] public int probaLootSalle1;
    [Range(0, 100)] public int probaLootSalle2;
    [Range(0, 100)] public int probaLootSalle3;
    [Range(0, 100)] public int probaLootSalle4;
    private int currentRoom;
    private int currentProba;

    [Header("Proba Weapon")] 
    [Range(0, 100)] public int probaWeapon;
    [Range(0, 100)] public int probaWeaponLvl1;
    [Range(0, 100)] public int probaWeaponLvl2;
    [Range(0, 100)] public int probaWeaponLvl3;
    
    [Header("Proba Module")]
    [Range(0, 100)] public int probaModule;
    [Range(0, 100)] public int probaModuleLvl1;
    [Range(0, 100)] public int probaModuleLvl2;
    [Range(0, 100)] public int probaModuleLvl3;


    private void Awake()
    {
        Instance = this;
    }


    public void EndRoom(Vector2 posSpawn)
    {
        if (currentRoom == 1)
        {
            currentProba = probaLootSalle1;
        }
        
        else if (currentRoom == 2)
        {
            currentProba = probaLootSalle2;
        }
        
        else if (currentRoom == 3)
        {
            currentProba = probaLootSalle3;
        }
        
        else if (currentRoom == 4)
        {
            currentProba = probaLootSalle4;
        }
        
        else
        {
            currentProba = 100;
        }


        int index = Random.Range(0, 100);
        bool isWeapon;

        if (index <= currentProba)
        {
            isWeapon = LootType();

            if (isWeapon)
            { 
                GameObject newWeapon = LootIsWeapon();

                Instantiate(newWeapon, posSpawn, Quaternion.identity);
            }

            else
            {
                GameObject newModule = LootIsModule();
                
                Instantiate(newModule, posSpawn, Quaternion.identity);
            }
        }
    }


    public bool LootType()
    {
        int index = Random.Range(0, 100);

        if (index < probaWeapon)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public GameObject LootIsWeapon()
    {
        int index = Random.Range(0, 100);

        if (index < probaWeaponLvl1)
        {
            return weaponsLevel1[Random.Range(0, weaponsLevel1.Count)];
        }
        
        else if (index < probaWeaponLvl1 + probaWeaponLvl2)
        {
            return weaponsLevel2[Random.Range(0, weaponsLevel2.Count)];
        }
        
        else
        {
            return weaponsLevel3[Random.Range(0, weaponsLevel3.Count)];
        }
    }


    public GameObject LootIsModule()
    {
        int index = Random.Range(0, 100);

        if (index < probaModuleLvl1)
        {
            return modulesLevel1[Random.Range(0, modulesLevel1.Count)];
        }
        
        else if (index < probaModuleLvl1 + probaModuleLvl2)
        {
            return modulesLevel2[Random.Range(0, modulesLevel2.Count)];
        }
        
        else
        {
            return modulesLevel3[Random.Range(0, modulesLevel3.Count)];
        }
    }
    
}
