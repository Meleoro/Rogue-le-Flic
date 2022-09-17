using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class GenerationPro : MonoBehaviour
{
    public static GenerationPro Instance;
    
    public int roomNumber;
    
    public Map map = new Map();
    
    [Header("Rooms")]
    public GameObject spawn;
    public GameObject room;
    public GameObject boss;
    
    [Header("Mini Map")]
    public GameObject miniSpawn;
    public GameObject miniRoom;
    public GameObject miniBoss;

    public Vector2 spawnLocation;
    private int saveX;
    private int saveY;
    private int newSaveX;
    private int newSaveY;
    private int saveDirection;

    private bool bossRoom;
    private float maxDistance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);
    }

    void Start()
    {
        GenerateMap();
        //DisplayMap();
    }
    
    public void GenerateMap()
    {
        for (int i = 0; i < roomNumber; i++)
        {
            // ON FAIT APPARAITRE LE SPAWN
            if (i == 0)
            {
                int x = Random.Range(3, 5);
                int y = Random.Range(3, 5);

                map.list[x].list[y] = spawn;

                saveX = x;
                saveY = y;

                spawnLocation = new Vector2(x, y);

                saveDirection = 5;
            }

            // BOUCLE PRINCIPALE QUI GENERE LA MAP
            else
            {
                for (int k = 0; k < 4; k++)
                {
                    // Détermination de la direction de la salle à ajouter
                    int direction = Random.Range(0, 4);
                    
                    while (direction == saveDirection)
                    {
                         direction = Random.Range(0, 4);
                    }

                    saveDirection = direction;

                    // Droite
                    if (direction == 0)
                    {
                        if (map.list[saveX + 1].list[saveY] == null)
                        {
                            map.list[saveX + 1].list[saveY] = room;

                            i += 1;

                            newSaveX = saveX + 1;
                            newSaveY = saveY;
                        }
                    }
                    
                    // Bas
                    else if (direction == 1)
                    {
                        if (map.list[saveX].list[saveY - 1] == null)
                        {
                            map.list[saveX].list[saveY - 1] = room;
                            
                            i += 1;

                            newSaveX = saveX;
                            newSaveY = saveY - 1;
                        }
                    }
                    
                    // Gauche
                    else if (direction == 2)
                    {
                        if (map.list[saveX - 1].list[saveY] == null)
                        {
                            map.list[saveX - 1].list[saveY] = room;
                            
                            i += 1;
                            
                            newSaveX = saveX - 1;
                            newSaveY = saveY;
                        }
                    }

                    // Haut
                    else
                    {
                        if (map.list[saveX].list[saveY + 1] == null)
                        {
                            map.list[saveX].list[saveY + 1] = room;
                            
                            i += 1;
                            
                            newSaveX = saveX;
                            newSaveY = saveY + 1;
                        }
                    }
                }

                saveX = newSaveX;
                saveY = newSaveY;
            }
        }
        
        // SALLE DE BOSS
        while (!bossRoom)
        {
            // Détermination de la direction de la salle à ajouter
            int direction = Random.Range(0, 4);
                    
            while (direction == saveDirection)
            {
                direction = Random.Range(0, 4);
            }

            saveDirection = direction;

            // Droite
            if (direction == 0)
            {
                if (map.list[saveX + 1].list[saveY] == null)
                {
                    map.list[saveX + 1].list[saveY] = boss;
                    
                    bossRoom = true;
                }
            }
                    
            // Bas
            else if (direction == 1)
            {
                if (map.list[saveX].list[saveY - 1] == null)
                {
                    map.list[saveX].list[saveY - 1] = boss;
                    
                    bossRoom = true;
                }
            }
                    
            // Gauche
            else if (direction == 2)
            {
                if (map.list[saveX - 1].list[saveY] == null)
                {
                    map.list[saveX - 1].list[saveY] = boss;

                    bossRoom = true;
                }
            }

            // Haut
            else
            {
                if (map.list[saveX].list[saveY + 1] == null)
                {
                    map.list[saveX].list[saveY + 1] = boss;
                    
                    bossRoom = true;
                }
            }
        }
    }

    
    public void DisplayMap()
    {
        for (int x = 0; x < map.list.Count; x++)
        {
            for (int y = 0; y < map.list.Count; y++)
            {
                if(map.list[x].list[y] != null)
                    Instantiate(map.list[x].list[y], new Vector3(x * 2 - map.list.Count, y * 2 - map.list.Count, 0), Quaternion.identity);
            }
        }
    }

    
    public void DistantRoom()
    {
        for (int x = 0; x < map.list.Count; x++)
        {
            for (int y = 0; y < map.list.Count; y++)
            {
                if(map.list[x].list[y] != null)
                    Instantiate(map.list[x].list[y], new Vector3(x * 2 - map.list.Count, y * 2 - map.list.Count, 0), Quaternion.identity);
            }
        }
    }
}
