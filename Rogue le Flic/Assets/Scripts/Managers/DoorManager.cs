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
    public int roomPosX;
    public int roomPosY;

    [Header("Autres")] 
    public int minEnnemies;
    public int maxEnnemies;
    public List<ennemySpawn> spawnEnnemies;
    public List<GameObject> currentEnnemies;


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
            foreach (ennemySpawn k in spawnEnnemies)
            {
                int index = Random.Range(0, 100);

                if (index < k.spawnChances && currentEnnemies.Count < maxEnnemies)
                {
                    currentEnnemies.Add(k.ennemy);
                }
            }

            if (currentEnnemies.Count >= minEnnemies)
                stopWhile = true;
        }
    }
}

[Serializable]
public class ennemySpawn
{
    public GameObject ennemy;
    public int spawnChances;
}
