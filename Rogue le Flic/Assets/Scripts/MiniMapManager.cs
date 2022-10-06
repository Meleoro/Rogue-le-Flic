using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoBehaviour
{
    public static MiniMapManager Instance;
    
    public Vector3 origin;

    public GameObject camera;
    public GameObject room;


    private void Awake()
    {
        Instance = this;
    }


    public void ActualiseMap(Map activeMap)
    {
        for (int i = 0; i < activeMap.list.Count; i++)
        {
            for (int k = 0; k < activeMap.list[i].list.Count; k++)
            {
                if (activeMap.list[i].list[k] != null)
                {
                    Vector2 newPos = origin + new Vector3(i, k, 0);
                    
                    Instantiate(room, newPos, Quaternion.identity);
                }
            }
        }

        camera.transform.position = origin + new Vector3(MapManager.Instance.playerX, MapManager.Instance.playerY, -10);
    }
}
