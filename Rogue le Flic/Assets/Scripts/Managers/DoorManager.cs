using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class DoorManager : MonoBehaviour
{
    public bool disableEndEffect;

    [Header("Doors")]
    public GameObject doorRight;
    public GameObject doorBottom;
    public GameObject doorLeft;
    public GameObject doorUp;

    [Header("Apparations Joueur")] 
    public GameObject right;
    public GameObject bottom;
    public GameObject left;
    public GameObject up;

    [Header("Coordonnés Room")] 
    [HideInInspector] public int roomPosX;
    [HideInInspector] public int roomPosY;

    [Header("Ennemies")] 
    public int minEnnemies;
    public int maxEnnemies;
    public List<spawnChance> spawnEnnemies;
    [HideInInspector] public List<GameObject> currentEnnemies;
    [HideInInspector] public int ennemyCount;

    [Header("Loot")]
    public List<spawnChance> spawnLoots;
    public GameObject healObject;
    public int probaDropHeal;
    [HideInInspector] public bool isFinished;
    private float timerFinish;
    private bool stopItem;

    [Header("BossRoom")]
    public bool bossRoom;
    private bool hasExitDoor;

    
    
    

    private void Start()
    {
        if (!GenerationPro.Instance.testLDMode)
        {
            if (ennemyCount != 0 || bossRoom)
            {
                doorRight.SetActive(false);
                doorLeft.SetActive(false);
                doorUp.SetActive(false);
                doorBottom.SetActive(false);
                
                PortesActivesRed();
            }

            else
            {
                PortesActivesGreen();
            }
        }
    }
    
    


    private void Update()
    {
        if (isFinished && timerFinish >= 0)
        {
            timerFinish -= Time.deltaTime;

            MapManager.Instance.winVolume.weight = timerFinish / 1.5f;
        }
    }


    public void PortesActivesGreen()
    {
        if (GenerationPro.Instance.map.list[roomPosX + 1].list[roomPosY] != null)
        {
            doorRight.SetActive(true);

            if(doorRight.CompareTag("NewDoor"))
                doorRight.GetComponent<NewDoor>().isOpen = true;
        }
        else
        {
            if (doorRight.GetComponent<NewDoor>().isFinalDoor)
            {
                doorRight.GetComponent<NewDoor>().isOpen = true;
            }
            else
            {
                doorRight.SetActive(false);
            }
        }
        
        if (GenerationPro.Instance.map.list[roomPosX].list[roomPosY - 1] != null)
        {
            doorBottom.SetActive(true);
            
            if(doorBottom.CompareTag("NewDoor"))
                doorBottom.GetComponent<NewDoor>().isOpen = true;
        }
        else
        {
            if (doorBottom.GetComponent<NewDoor>().isFinalDoor)
            {
                doorBottom.GetComponent<NewDoor>().isOpen = true;
            }
            else
            {
                doorBottom.SetActive(false);
            }
        }
        
        if (GenerationPro.Instance.map.list[roomPosX].list[roomPosY + 1] != null)
        {
            if(doorUp.CompareTag("NewDoor"))
                doorUp.GetComponent<NewDoor>().isOpen = true;
            
            doorUp.SetActive(true);
        }
        else
        {
            if (doorUp.GetComponent<NewDoor>().isFinalDoor)
            {
                doorUp.GetComponent<NewDoor>().isOpen = true;
            }
            else
            {
                doorUp.SetActive(false);
            }
        }
        
        if (GenerationPro.Instance.map.list[roomPosX - 1].list[roomPosY] != null)
        {
            if(doorLeft.CompareTag("NewDoor"))
                doorLeft.GetComponent<NewDoor>().isOpen = true;
            
            doorLeft.SetActive(true);
        }
        else
        {
            if (doorLeft.GetComponent<NewDoor>().isFinalDoor)
            {
                doorLeft.GetComponent<NewDoor>().isOpen = true;
            }
            else
            {
                doorLeft.SetActive(false);
            }
        }
    }


    public void PortesActivesRed()
    {
        if (GenerationPro.Instance.map.list[roomPosX + 1].list[roomPosY] != null)
        {
            doorRight.SetActive(true);

            if(doorRight.CompareTag("NewDoor"))
                doorRight.GetComponent<NewDoor>().isOpen = false;
        }
        else
        {
            if(!hasExitDoor && bossRoom)
            {
                hasExitDoor = true;

                doorRight.SetActive(true);
                doorRight.GetComponent<NewDoor>().isFinalDoor = true;

                doorRight.GetComponent<NewDoor>().isOpen = false;
            }
            else
            {
                doorRight.SetActive(false);
            }
        }
        
        if (GenerationPro.Instance.map.list[roomPosX].list[roomPosY - 1] != null)
        {
            doorBottom.SetActive(true);
            
            if(doorBottom.CompareTag("NewDoor"))
                doorBottom.GetComponent<NewDoor>().isOpen = false;
        }
        else
        {
            if (!hasExitDoor && bossRoom)
            {
                hasExitDoor = true;

                doorBottom.SetActive(true);
                doorBottom.GetComponent<NewDoor>().isFinalDoor = true;
                
                doorBottom.GetComponent<NewDoor>().isOpen = false;
            }
            else
            {
                doorBottom.SetActive(false);
            }
        }
        
        if (GenerationPro.Instance.map.list[roomPosX].list[roomPosY + 1] != null)
        {
            if(doorUp.CompareTag("NewDoor"))
                doorUp.GetComponent<NewDoor>().isOpen = false;
            
            doorUp.SetActive(true);
        }
        else
        {
            if (!hasExitDoor && bossRoom)
            {
                hasExitDoor = true;

                doorUp.SetActive(true);
                doorUp.GetComponent<NewDoor>().isFinalDoor = true;
                
                doorUp.GetComponent<NewDoor>().isOpen = false;
            }
            else
            {
                doorUp.SetActive(false);
            }
        }
        
        if (GenerationPro.Instance.map.list[roomPosX - 1].list[roomPosY] != null)
        {
            if(doorLeft.CompareTag("NewDoor"))
                doorLeft.GetComponent<NewDoor>().isOpen = false;
            
            doorLeft.SetActive(true);
        }
        else
        {
            if (!hasExitDoor && bossRoom)
            {
                hasExitDoor = true;

                doorLeft.SetActive(true);
                doorLeft.GetComponent<NewDoor>().isFinalDoor = true;
                
                doorLeft.GetComponent<NewDoor>().isOpen = false;
            }
            else
            {
                doorLeft.SetActive(false);
            }
        }
    }

    
    public void GenerateEnnemies()
    {
        bool stopWhile = false;

        while (!stopWhile)
        {
            foreach (spawnChance k in spawnEnnemies)
            {
                int index = Random.Range(0, 100);

                if (index < k.spawnChances && currentEnnemies.Count < maxEnnemies)
                {
                    
                    currentEnnemies.Add(k.element);
                    
                    k.spawnChances = 0;
                }
            }

            if (currentEnnemies.Count >= minEnnemies)
                stopWhile = true;
        }

        ennemyCount = currentEnnemies.Count;
    }

    
    public void EndRoom(Vector2 posSpawn)
    {
        if (!disableEndEffect)
        {
            bool createdItem = false;
            stopItem = false;

            // DROP HEAL
            int indexHeal = Random.Range(0, 100);

            if (indexHeal <= probaDropHeal)
            {
                GameObject heal = Instantiate(healObject, posSpawn, Quaternion.identity);

                Vector2 randomDirection = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));

                heal.GetComponentInParent<Rigidbody2D>().AddForce(randomDirection.normalized * 4, ForceMode2D.Impulse);
            }
            
            
            /*// DROP ITEM
            foreach (spawnChance k in spawnLoots)
            {
                int index = Random.Range(0, 100);

                if (index < k.spawnChances && !stopItem)
                {
                    Instantiate(k.element, posSpawn, Quaternion.identity);
                    createdItem = true;

                    stopItem = true;
                }
            }
            
            if (!createdItem)
            {
                for (int i = 0; i < 6; i++)
                {
                    Instantiate(CoinManager.Instance.coin, posSpawn, quaternion.identity);
                }
            }

            timerFinish = 1;*/
            
            for (int i = 0; i < MoneyManager.Instance.moneyDropPerEnnemy; i++)
            {
                Instantiate(CoinManager.Instance.coin, posSpawn, quaternion.identity);
            }
            
            LootManager.Instance.EndRoom(posSpawn);

            PortesActivesGreen();
        }
    }
}

[Serializable]
public class spawnChance
{
    public GameObject element;
    public int spawnChances;
}
