using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class DoorManager : MonoBehaviour
{
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
    [HideInInspector] public bool isFinished;
    private float timerFinish;


    private void Start()
    {
        if (ennemyCount != 0)
        {
            doorBottom.SetActive(false);
            doorRight.SetActive(false);
            doorLeft.SetActive(false);
            doorUp.SetActive(false);
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


    public void PortesActives()
    {
        if (GenerationPro.Instance.map.list[roomPosX + 1].list[roomPosY] != null)
        {
            doorRight.SetActive(true);
        }
        else
        {
            doorRight.SetActive(false);
        }
        
        if (GenerationPro.Instance.map.list[roomPosX].list[roomPosY - 1] != null)
        {
            doorBottom.SetActive(true);
        }
        else
        {
            doorBottom.SetActive(false);
        }
        
        if (GenerationPro.Instance.map.list[roomPosX].list[roomPosY + 1] != null)
        {
            doorUp.SetActive(true);
        }
        else
        {
            doorUp.SetActive(false);
        }
        
        if (GenerationPro.Instance.map.list[roomPosX - 1].list[roomPosY] != null)
        {
            doorLeft.SetActive(true);
        }
        else
        {
            doorLeft.SetActive(false);
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
        bool stopWhile = false;

        while (!stopWhile)
        {
            foreach (spawnChance k in spawnLoots)
            {
                int index = Random.Range(0, 100);

                if (index < k.spawnChances && !stopWhile)
                {
                    stopWhile = true;

                    Instantiate(k.element, posSpawn, Quaternion.identity);
                }
            }
        }

        timerFinish = 1;
        
        PortesActives();
    }
}

[Serializable]
public class spawnChance
{
    public GameObject element;
    public int spawnChances;
}
