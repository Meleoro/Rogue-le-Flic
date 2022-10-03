using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    
    private bool spawnRoom;

    public int playerX;
    public int playerY;

    public GameObject activeRoom;
    
    public Map mapActive = new Map();


    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Spawn();
    }

    void Update()
    {
        /*if (!spawnRoom)
        {
            Spawn();

            spawnRoom = true;
        }*/
    }

    void Spawn()
    {
        int x = (int) GenerationPro.Instance.spawnLocation.x;
        int y = (int) GenerationPro.Instance.spawnLocation.y;

        playerX = x;
        playerY = y;

        activeRoom = Instantiate(GenerationPro.Instance.map.list[x].list[y], Vector3.zero, Quaternion.identity);
        
        mapActive.list[x].list[y] = activeRoom;
        
        activeRoom.GetComponent<DoorManager>().roomPosX = playerX;
        activeRoom.GetComponent<DoorManager>().roomPosY = playerY;
        
        activeRoom.GetComponent<DoorManager>().PortesActives();
        
        EnterRoom(activeRoom.GetComponent<DoorManager>().currentEnnemies);
    }


    public void ChangeRoom(int doorNumber)
    {
        mapActive.list[playerX].list[playerY].SetActive(false);

        ExitRoom(activeRoom.GetComponent<DoorManager>().currentEnnemies);
        
        
        if (doorNumber == 0)
        {
            playerX += 1;

            if (mapActive.list[playerX].list[playerY] == null)
            {
                activeRoom = Instantiate(GenerationPro.Instance.map.list[playerX].list[playerY], Vector3.zero,
                    Quaternion.identity);
                
                mapActive.list[playerX].list[playerY] = activeRoom;
                
                activeRoom.GetComponent<DoorManager>().GenerateEnnemies();
            }
            
            else
            {
                activeRoom = mapActive.list[playerX].list[playerY];
                
                mapActive.list[playerX].list[playerY].SetActive(true);
            }
            

            MovementsChara.Instance.transform.position = activeRoom.GetComponent<DoorManager>().left.transform.position;
        }

        else if (doorNumber == 1)
        {
            playerY -= 1;

            if (mapActive.list[playerX].list[playerY] == null)
            {
                activeRoom = Instantiate(GenerationPro.Instance.map.list[playerX].list[playerY], Vector3.zero,
                    Quaternion.identity);
                
                mapActive.list[playerX].list[playerY] = activeRoom;
                
                activeRoom.GetComponent<DoorManager>().GenerateEnnemies();
            }

            else
            {
                activeRoom = mapActive.list[playerX].list[playerY];
                
                mapActive.list[playerX].list[playerY].SetActive(true);
            }

            MovementsChara.Instance.transform.position = activeRoom.GetComponent<DoorManager>().up.transform.position;
        }

        else if (doorNumber == 2)
        {
            playerX -= 1;

            if (mapActive.list[playerX].list[playerY] == null)
            {
                activeRoom = Instantiate(GenerationPro.Instance.map.list[playerX].list[playerY], Vector3.zero,
                    Quaternion.identity);
            
                mapActive.list[playerX].list[playerY] = activeRoom;
                
                activeRoom.GetComponent<DoorManager>().GenerateEnnemies();
            }

            else
            {
                activeRoom = mapActive.list[playerX].list[playerY];
                
                mapActive.list[playerX].list[playerY].SetActive(true);
            }
            
            MovementsChara.Instance.transform.position = activeRoom.GetComponent<DoorManager>().right.transform.position;
        }

        else
        {
            playerY += 1;

            if (mapActive.list[playerX].list[playerY] == null)
            {
                activeRoom = Instantiate(GenerationPro.Instance.map.list[playerX].list[playerY], Vector3.zero,
                    Quaternion.identity);
            
                mapActive.list[playerX].list[playerY] = activeRoom;
                
                activeRoom.GetComponent<DoorManager>().GenerateEnnemies();
            }

            else
            {
                activeRoom = mapActive.list[playerX].list[playerY];
                
                mapActive.list[playerX].list[playerY].SetActive(true);
            }
            
            MovementsChara.Instance.transform.position = activeRoom.GetComponent<DoorManager>().bottom.transform.position;
        }

        activeRoom.GetComponent<DoorManager>().roomPosX = playerX;
        activeRoom.GetComponent<DoorManager>().roomPosY = playerY;

        activeRoom.GetComponent<DoorManager>().PortesActives();
        
        EnterRoom(activeRoom.GetComponent<DoorManager>().currentEnnemies);
    }


    void EnterRoom(List<GameObject> ennemies)
    {
        foreach (GameObject k in ennemies)
        {
            if(k != null)
                k.SetActive(true);
        }
        
        ReferenceCamera.Instance.camera.orthographicSize = activeRoom.GetComponent<CameraManager>().cameraSize;
    }

    void ExitRoom(List<GameObject> ennemies)
    {
        foreach (GameObject k in ennemies)
        {
            if(k != null)
                k.SetActive(false);
        }
    }
}
