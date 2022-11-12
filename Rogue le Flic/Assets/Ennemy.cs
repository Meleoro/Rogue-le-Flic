using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    public enum ennemies
    {
        Beaver,
        Frog,
        Turtle
    }

    public ennemies ennemyType;

    public GameObject cible;
    [SerializeField] private GameObject spawnIndicator;

    private Beaver beaverScript;
    private Frog frogScript;
    private Turtle turtleScript;

    [HideInInspector] public bool isCharging;
    private bool isSpawning;


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

            case ennemies.Turtle:
                turtleScript = GetComponent<Turtle>();
                break;
        }

        StartCoroutine(Spawn());
    }


    private void Update()
    {
        if (!isSpawning)
        {
            switch (ennemyType)
            {
                case ennemies.Beaver :
                    beaverScript.BeaverBehavior();
                    break;
            
                case ennemies.Frog :
                    frogScript.FrogBehavior();
                    break;

                case ennemies.Turtle :
                    turtleScript.TurtleBehavior();
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isSpawning)
        {
            switch (ennemyType)
            {
                case ennemies.Beaver :
                    beaverScript.BeaverFixedBehavior();
                    break;
                
                case ennemies.Frog :
                    frogScript.FrogFixedBehavior();
                    break;

                case ennemies.Turtle:
                    turtleScript.TurtleFixedBehavior();
                    break;
            }
        }
    }


    public void TakeDamages(int damages, GameObject bullet)
    {
        switch (ennemyType)
        {
            case ennemies.Beaver:
                beaverScript.TakeDamages(damages, bullet);
                break;

            case ennemies.Frog:
                frogScript.TakeDamages(damages, bullet);
                break;

            case ennemies.Turtle:
                turtleScript.TakeDamages(damages, bullet);
                break;
        }
    }


    private IEnumerator Spawn()
    {
        spawnIndicator.SetActive(true);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        isSpawning = true;
        
        yield return new WaitForSeconds(2);
        
        spawnIndicator.SetActive(false);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        
        isSpawning = false;
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

            case ennemies.Turtle:
                turtleScript.StopCoroutine();
                break;
        }
    }
}
