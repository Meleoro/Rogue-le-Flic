using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoBehaviour
{
    public static MiniMapManager Instance;
    
    public Vector3 origin;

    public GameObject cameraMini;
    public GameObject room;
    
    List<GameObject> activeRooms = new List<GameObject>();
    
    public Sprite normalRoom;
    public Sprite shopRoom;
    public Sprite bossRoom;


    private void Awake()
    {
        Instance = this;
    }


    public void ActualiseMap(Map activeMap)
    {
        foreach (GameObject k in activeRooms)
        {
            Destroy(k);
        }
        
        activeRooms.Clear();
        
        for (int i = 0; i < activeMap.list.Count; i++)
        {
            for (int k = 0; k < activeMap.list[i].list.Count; k++)
            {
                if (activeMap.list[i].list[k] != null)
                {
                    Vector2 newPos = origin + new Vector3(i, k, 0);

                    GameObject newRoom = Instantiate(room, newPos, Quaternion.identity);

                    activeRooms.Add(newRoom);

                    ActivateDoors(i, k, newRoom);

                    if (activeMap.list[i].list[k].CompareTag("BossRoom"))
                    {
                        newRoom.GetComponent<SpriteRenderer>().sprite = bossRoom;
                    }
                    
                    else if (activeMap.list[i].list[k].CompareTag("ShopRoom"))
                    {
                        newRoom.GetComponent<SpriteRenderer>().sprite = shopRoom;
                    }

                    else
                    {
                        newRoom.GetComponent<SpriteRenderer>().sprite = normalRoom;
                    }
                }
            }
        }

        cameraMini.transform.position = origin + new Vector3(MapManager.Instance.playerX, MapManager.Instance.playerY, -10);
    }

    
    void ActivateDoors(int x, int y, GameObject miniMapRoom)
    {
        MiniMapRoom script = miniMapRoom.GetComponent<MiniMapRoom>();

        if (GenerationPro.Instance.map.list[x + 1].list[y] != null)
        {
            script.rightDoor.SetActive(true);
        }
        else
        {
            script.rightDoor.SetActive(false);
        }

        if (GenerationPro.Instance.map.list[x].list[y - 1] != null)
        {
            script.downDoor.SetActive(true);
        }
        else
        {
            script.downDoor.SetActive(false);
        }

        if (GenerationPro.Instance.map.list[x].list[y + 1] != null)
        {
            script.upDoor.SetActive(true);
        }
        else
        {
            script.upDoor.SetActive(false);
        }

        if (GenerationPro.Instance.map.list[x - 1].list[y] != null)
        {
            script.leftDoor.SetActive(true);
        }
        else
        {
            script.leftDoor.SetActive(false);
        }
    }
}
