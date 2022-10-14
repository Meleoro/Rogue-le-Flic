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

    public Beaver beaverScript;
    public Frog frogScript;
    

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
                break;
        }
    }


    public void StopCoroutines()
    {
        switch (ennemyType)
        {
            case ennemies.Beaver :
                beaverScript.StopCoroutine();
                break;
            
            case ennemies.Frog :
                frogScript.StopAllCoroutines();
                break;
        }
    }
}
