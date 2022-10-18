using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    public enum ennemies
    {
        Beaver,
        Frog
    }

    public ennemies ennemyType;

    private Beaver beaverScript;
    private Frog frogScript;

    [HideInInspector] public bool isCharging;


    private void Start()
    {
        switch (ennemyType)
        {
            case ennemies.Beaver :
                beaverScript = GetComponent<Beaver>();
                break;
            
            case ennemies.Frog :
                frogScript = GetComponent<Frog>();
                break;
        }
    }


    private void Update()
    {
        switch (ennemyType)
        {
            case ennemies.Beaver :
                beaverScript.BeaverBehavior();
                break;
            
            case ennemies.Frog :
                frogScript.FrogBehavior();
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (ennemyType)
        {
            case ennemies.Beaver :
                beaverScript.BeaverFixedBehavior();
                break;
            
            case ennemies.Frog :
                frogScript.FrogFixedBehavior();
                break;
        }
    }


    public void StopCoroutines()
    {
        isCharging = false;
        
        switch (ennemyType)
        {
            case ennemies.Beaver :
                beaverScript.StopCoroutine();
                break;
            
            case ennemies.Frog :
                frogScript.StopCoroutine();
                break;
        }
    }
}
