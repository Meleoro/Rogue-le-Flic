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

    [Header("Other")] 
    private bool weaponChosen;
    private bool moduleChosen;
    private int currentWeapon1;
    private int currentWeapon2;
    private int currentModule1;
    private int currentModule2;
    

    private void Awake()
    {
        Instance = this;

        currentRoom = 0;
    }


    public void EndRoom(Vector2 posSpawn)
    {
        currentRoom += 1;
        
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

                currentRoom = 0;
            }

            else
            {
                GameObject newModule = LootIsModule();
                
                Instantiate(newModule, posSpawn, Quaternion.identity);
                
                currentRoom = 0;
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

        int weaponSelected = 0;
        weaponChosen = false;
        
        if (ManagerChara.Instance.activeGun != null)
        {
            currentWeapon1 = ManagerChara.Instance.activeGun.GetComponent<Gun>().weaponType;
        }

        if (ManagerChara.Instance.stockWeapon != null)
        {
            currentWeapon2 = ManagerChara.Instance.stockWeapon.GetComponent<Gun>().weaponType;
        }

        while (!weaponChosen)
        {
            weaponSelected = Random.Range(1, 4);

            if (weaponSelected != currentWeapon1 && weaponSelected != currentWeapon2)
            {
                weaponChosen = true;
            }
        }
        
        if (index < probaWeaponLvl1)
        {
            return weaponsLevel1[weaponSelected - 1];
        }
        
        else if (index < probaWeaponLvl1 + probaWeaponLvl2)
        {
            return weaponsLevel2[weaponSelected - 1];
        }
        
        else
        {
            return weaponsLevel3[weaponSelected - 1];
        }
    }


    public GameObject LootIsModule()
    {
        int index = Random.Range(0, 100);

        int moduleSelected = 0;
        moduleChosen = false;

        currentModule1 = ModuleManager.Instance.Module1;
        currentModule2 = ModuleManager.Instance.Module2;

        while (!moduleChosen)
        {
            moduleSelected = Random.Range(1, 6);

            if (moduleSelected != currentModule1 && moduleSelected != currentModule2)
            {
                moduleChosen = true;
            }
        }

        if (index < probaModuleLvl1)
        {
            return modulesLevel1[moduleSelected - 1];
        }
        
        else if (index < probaModuleLvl1 + probaModuleLvl2)
        {
            return modulesLevel2[moduleSelected - 1];
        }
        
        else
        {
            return modulesLevel3[moduleSelected - 1];
        }
    }
    
}
