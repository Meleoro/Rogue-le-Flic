using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TokenAnimation : MonoBehaviour
{
    public GameObject[] hazards;

    void Start()
    {
        GameObject hazard = hazards[Random.Range(0, hazards.Length)];
        Vector3[] spawnPositions = new[] { new Vector3(20, 5, 0), new Vector3(20, 3, 0), new Vector3(20, 1, 0), new Vector3(20, -1, 0), new Vector3(20, -3, 0), new Vector3(20, -5, 0) };
        Quaternion spawnRotation = Quaternion.identity;
        for(int i = 1; i < 6; i++)
        {
            Instantiate(hazard, spawnPositions[i], spawnRotation);
        }
    }
    
    
}


