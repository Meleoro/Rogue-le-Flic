using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    
    private bool spawnRoom;

    public int playerX;
    public int playerY;


    void Awake()
    {
        Instance = this;
    }
    
    void Update()
    {
        if (!spawnRoom)
        {
            Spawn();

            spawnRoom = true;
        }
    }

    void Spawn()
    {
        int x = (int) GenerationPro.Instance.spawnLocation.x;
        int y = (int) GenerationPro.Instance.spawnLocation.y;

        playerX = x;
        playerY = y;

        Instantiate(GenerationPro.Instance.map.list[x].list[y], Vector3.zero, Quaternion.identity);
    }


    public void ChangeRoom(int doorNumber)
    {
        if (doorNumber == 0)
        {
            playerX -= 1;
            
            Instantiate(GenerationPro.Instance.map.list[playerX].list[playerY], Vector3.zero, Quaternion.identity);
        }
        
        else if (doorNumber == 1)
        {
            playerY -= 1;
            
            Instantiate(GenerationPro.Instance.map.list[playerX].list[playerY], Vector3.zero, Quaternion.identity);
        }
        
        else if (doorNumber == 2)
        {
            playerX += 1;
            
            Instantiate(GenerationPro.Instance.map.list[playerX].list[playerY], Vector3.zero, Quaternion.identity);
        }

        else
        {
            playerY += 1;
            
            Instantiate(GenerationPro.Instance.map.list[playerX].list[playerY], Vector3.zero, Quaternion.identity);
        }
    }
}
