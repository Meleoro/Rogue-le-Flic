using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class GenerationPro : MonoBehaviour
{
    public bool testLDMode;

    public static GenerationPro Instance;
    
    public int roomNumber;
    
    public Map map = new Map();
    
    [Header("Rooms")]
    public GameObject spawn;
    public List<GameObject> basicRooms;
    public List<GameObject> bigRooms;
    public List<GameObject> specialRooms;
    public List<Coord> roomsCreated;
    public GameObject boss;
    
    [Header("Autres")]
    public Vector2 spawnLocation;
    private int saveX;
    private int saveY;
    private int newSaveX;
    private int newSaveY;
    private int saveDirection;
    
    [Header("BigRoom")]
    private int voisinsNbr;
    private int boucleNbr;
    private int nbrBigRoom;

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
        if (!testLDMode)
        {
            GenerateMap();

            boucleNbr = 0;
        }
    }
    
    public void GenerateMap()
    {
        for (int i = 0; i < roomNumber; i++)
        {
            boucleNbr += 1;
            
            // ON FAIT APPARAITRE LE SPAWN
            if (i == 0)
            {
                int x = 5;
                int y = 5;

                map.list[x].list[y] = spawn;

                saveX = x;
                saveY = y;

                spawnLocation = new Vector2(x, y);

                saveDirection = 5;
            }

            // BOUCLE PRINCIPALE QUI GENERE LA MAP
            else
            {
                voisinsNbr = 0;
                
                for (int k = 0; k < 4; k++)
                {
                    // Détermination de la direction de la salle à ajouter et de la salle en elle-même
                    int direction = Random.Range(0, 4);
                    int generatedRoom = Random.Range(0, basicRooms.Count);
                    
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
                            map.list[saveX + 1].list[saveY] = basicRooms[generatedRoom];

                            i += 1;

                            newSaveX = saveX + 1;
                            newSaveY = saveY;

                            voisinsNbr += 1;


                            roomsCreated.Add(new Coord());

                            roomsCreated[roomsCreated.Count - 1].x = newSaveX;
                            roomsCreated[roomsCreated.Count - 1].y = newSaveY;
                        }
                    }
                    
                    // Bas
                    else if (direction == 1)
                    {
                        if (map.list[saveX].list[saveY - 1] == null)
                        {
                            map.list[saveX].list[saveY - 1] = basicRooms[generatedRoom];
                            
                            i += 1;

                            newSaveX = saveX;
                            newSaveY = saveY - 1;
                            
                            voisinsNbr += 1;


                            roomsCreated.Add(new Coord());

                            roomsCreated[roomsCreated.Count - 1].x = newSaveX;
                            roomsCreated[roomsCreated.Count - 1].y = newSaveY;
                        }
                    }
                    
                    // Gauche
                    else if (direction == 2)
                    {
                        if (map.list[saveX - 1].list[saveY] == null)
                        {
                            map.list[saveX - 1].list[saveY] = basicRooms[generatedRoom];
                            
                            i += 1;
                            
                            newSaveX = saveX - 1;
                            newSaveY = saveY;
                            
                            voisinsNbr += 1;


                            roomsCreated.Add(new Coord());    

                            roomsCreated[roomsCreated.Count - 1].x = newSaveX;
                            roomsCreated[roomsCreated.Count - 1].y = newSaveY;
                        }
                    }

                    // Haut
                    else
                    {
                        if (map.list[saveX].list[saveY + 1] == null)
                        {
                            map.list[saveX].list[saveY + 1] = basicRooms[generatedRoom];
                            
                            i += 1;
                            
                            newSaveX = saveX;
                            newSaveY = saveY + 1;
                            
                            voisinsNbr += 1;


                            roomsCreated.Add(new Coord());

                            roomsCreated[roomsCreated.Count - 1].x = newSaveX;
                            roomsCreated[roomsCreated.Count - 1].y = newSaveY;
                        }
                    }
                }

                /*if (voisinsNbr >= 2 && boucleNbr >= 3 && nbrBigRoom < 1)
                {
                    nbrBigRoom += 1;

                    map.list[saveX].list[saveY] = bigRooms[0];
                }*/
                
                saveX = newSaveX;
                saveY = newSaveY;
            }
        }

        SpecialRooms();
        
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



    void SpecialRooms()
    {
        for(int k = 0; k < specialRooms.Count; k++)
        {
            for(int i = 2; i < roomsCreated.Count; i++)
            {
                Debug.Log(roomsCreated[i].x);
                Debug.Log(roomsCreated[i].y);
                
                // DROITE
                if (map.list[roomsCreated[i].x + 1].list[roomsCreated[i].y] == null && i < roomsCreated.Count)
                {
                    map.list[roomsCreated[i].x + 1].list[roomsCreated[i].y] = specialRooms[k];
                    i += roomsCreated.Count;
                }

                // BAS
                else if (map.list[roomsCreated[i].x].list[roomsCreated[i].y - 1] == null && i < roomsCreated.Count)
                {
                    map.list[roomsCreated[i].x].list[roomsCreated[i].y - 1] = specialRooms[k];
                    i += roomsCreated.Count;
                }

                // GAUCHE
                else if (map.list[roomsCreated[i].x - 1].list[roomsCreated[i].y] == null && i < roomsCreated.Count)
                {
                    map.list[roomsCreated[i].x - 1].list[roomsCreated[i].y] = specialRooms[k];
                    i += roomsCreated.Count;
                }

                // HAUT
                else if (map.list[roomsCreated[i].x].list[roomsCreated[i].y + 1] == null && i < roomsCreated.Count)
                {
                    map.list[roomsCreated[i].x].list[roomsCreated[i].y + 1] = specialRooms[k];
                    i += roomsCreated.Count;
                }
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

[Serializable]
public class Coord
{
    public int x;
    public int y;
}
