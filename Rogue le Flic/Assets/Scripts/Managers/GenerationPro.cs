using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class GenerationPro : MonoBehaviour
{
    public int roomNumber;
    
    public Map map = new Map();
    
    public GameObject spawn;
    public GameObject room;


    private int saveX;
    private int saveY;


    void Start()
    {
        GenerateMap();
    }
    
    public void GenerateMap()
    {
        for (int i = 0; i < roomNumber; i++)
        {
            if (i == 0)
            {
                int x = Random.Range(1, 3);
                int y = Random.Range(1, 3);

                map.list[x].list[y] = spawn;
                
                saveX = x;
                saveY = y;
            }

            else
            {
                for (int k = 0; k < 4; k++)
                {
                    int direction = Random.Range(0, 3);

                    
                    Debug.Log(12);
                    
                    if (direction == 0)
                    {
                        Debug.Log(12);
                        if (map.list[saveX + 1].list[saveY] == null)
                        {
                            map.list[saveX + 1].list[saveY] = room;
                            
                            i += 1;
                        }
                    }
                    
                    else if (direction == 1)
                    {
                        if (map.list[saveX].list[saveY - 1] == null)
                        {
                            map.list[saveX].list[saveY - 1] = room;
                            
                            i += 1;
                        }
                    }
                    
                    else if (direction == 2)
                    {
                        if (map.list[saveX - 1].list[saveY] == null)
                        {
                            map.list[saveX - 1].list[saveY] = room;
                            
                            i += 1;
                        }
                    }

                    else
                    {
                        if (map.list[saveX].list[saveY + 1] == null)
                        {
                            map.list[saveX].list[saveY + 1] = room;
                            
                            i += 1;
                        }
                    }
                }
            }
        }
    }
}
