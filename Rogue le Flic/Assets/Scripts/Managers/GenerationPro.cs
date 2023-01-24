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
    public bool bossFloor;

    public static GenerationPro Instance;
    
    public int roomNumber;
    
    public Map map = new Map();
    
    [Header("Rooms")]
    public GameObject spawn;
    public List<GameObject> basicRooms;
    //public List<GameObject> bigRooms;
    public List<GameObject> specialRooms;
    public List<Coord> roomsCreated;
    public List<GameObject> bossRooms;

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

    [Header("BossRoom")]
    private bool bossRoom;
    private bool roomSelected;
    private int indexBossRoom;

    private float maxDistance;
    private Vector2 farestRoom;
    private Vector2 directionFarestRoom;
    private int directionChoisie;
    
    

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

        if (!testLDMode)
        {
            //Lancer la musique de combat
            Music.Instance.TutoACombat();
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
                int x = 6;
                int y = 6;

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

                            basicRooms.RemoveAt(generatedRoom);
                        }
                    }
                    
                    // Bas
                    else if (direction == 1 && !bossFloor)
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

                            basicRooms.RemoveAt(generatedRoom);
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

                            basicRooms.RemoveAt(generatedRoom);
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

                            basicRooms.RemoveAt(generatedRoom);
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


        // ROOM SELECTION
        while (!roomSelected)
        {
            indexBossRoom = Random.Range(0, bossRooms.Count);

            if (!LevelManager.Instance.banishedRooms.Contains(indexBossRoom))
            {
                roomSelected = true;
            }
        }

        DistantRoom();

        saveDirection = -1;

        // SALLE DE BOSS
        while (!bossRoom)
        {
            if (!bossRoom)
            {
                // Détermination de la direction de la salle à ajouter
                int direction = Random.Range(0, 4);

                while (direction == saveDirection)
                {
                    direction = Random.Range(0, 4);
                }
                
                if (directionChoisie < 4 && saveDirection == -1)
                {
                    direction = directionChoisie;
                }
                
                

                saveDirection = direction;

                // Droite
                if (direction == 0)
                {
                    if (map.list[saveX + 1].list[saveY] == null)
                    {
                        newSaveX = saveX + 1;
                        newSaveY = saveY;

                        map.list[saveX + 1].list[saveY] = bossRooms[indexBossRoom];

                        bossRoom = true;
                    }
                }

                // Bas
                else if (direction == 1 && !bossFloor)
                {
                    if (map.list[saveX].list[saveY - 1] == null)
                    {
                        newSaveX = saveX;
                        newSaveY = saveY - 1;

                        map.list[saveX].list[saveY - 1] = bossRooms[indexBossRoom];

                        bossRoom = true;
                    }
                }

                // Gauche
                else if (direction == 2)
                {
                    if (map.list[saveX - 1].list[saveY] == null)
                    {
                        newSaveX = saveX - 1;
                        newSaveY = saveY;

                        map.list[saveX - 1].list[saveY] = bossRooms[indexBossRoom];

                        bossRoom = true;
                    }
                }

                // Haut
                else
                {
                    if (map.list[saveX].list[saveY + 1] == null)
                    {
                        newSaveX = saveX;
                        newSaveY = saveY + 1;

                        map.list[saveX].list[saveY + 1] = bossRooms[indexBossRoom];

                        bossRoom = true;
                    }
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
        maxDistance = 0;
        
        for(int i = 0; i < roomsCreated.Count; i++)
        {
            float currentDistance = Vector2.Distance(new Vector2(roomsCreated[i].x, roomsCreated[i].y), spawnLocation);

            if (currentDistance > maxDistance)
            {
                farestRoom = new Vector2(roomsCreated[i].x, roomsCreated[i].y);
                maxDistance = currentDistance;
            }
        }

        directionFarestRoom = farestRoom - spawnLocation;

        if (Mathf.Abs(directionFarestRoom.x) > Mathf.Abs(directionFarestRoom.y))
        {
            if (directionFarestRoom.x > 0)
            {
                directionChoisie = 0;
            }

            else
            {
                directionChoisie = 2;
            }
        }
        
        else if (Mathf.Abs(directionFarestRoom.x) < Mathf.Abs(directionFarestRoom.y))
        {
            if (directionFarestRoom.y < 0)
            {
                directionChoisie = 1;
            }

            else
            {
                directionChoisie = 3;
            }
        }

        else
        {
            directionChoisie = 5;
        }


        saveX = (int) farestRoom.x;
        saveY = (int) farestRoom.y;
    }
}

[Serializable]
public class Coord
{
    public int x;
    public int y;
}
