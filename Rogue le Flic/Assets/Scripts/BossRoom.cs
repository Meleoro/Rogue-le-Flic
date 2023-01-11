using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public List<Transform> spawnPoints;

    public bool isLastBoss;

    private void Start()
    {
        if (isLastBoss)
        {
            for (int k = 0; k < LevelManager.Instance.savedBoss.Count; k++)
            {
                GameObject newBoss = null;
                
                if(k == 0)
                    newBoss = Instantiate(LevelManager.Instance.savedBoss[k], transform.position + new Vector3(3, 0, 0), Quaternion.identity);
                
                else if (k == 1)
                    newBoss = Instantiate(LevelManager.Instance.savedBoss[k], transform.position + new Vector3(-3, 0, 0), Quaternion.identity);

                newBoss.GetComponent<Boss>().bossNumber = k + 1;
            }
        }
    }
}
