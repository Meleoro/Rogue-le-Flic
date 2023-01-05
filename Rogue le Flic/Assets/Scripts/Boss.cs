using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public enum boss
    {
        Beaver,
        Frog,
        Turtle
    }

    public boss bossType;

    private BeaverBoss beaverScript;
    private FrogBoss frogScript;
    private Turtle turtleScript;

    [Header("References")]
    public Animator anim;
    public GameObject sprite;
    public GameObject spawnIndicator;
    public BoxCollider2D _collider2D;


    private void Start()
    {
        switch (bossType)
        {
            case boss.Beaver:
                beaverScript = GetComponent<BeaverBoss>();
                break;

            case boss.Frog:
                frogScript = GetComponent<FrogBoss>();
                break;

            case boss.Turtle:
                turtleScript = GetComponent<Turtle>();
                break;
        }
    }


    private void Update()
    {
        switch (bossType)
        {
            case boss.Beaver:
                beaverScript.BeaverBehavior();
                break;
            
            case boss.Frog:
                frogScript.FrogBehavior();
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (bossType)
        {
            case boss.Beaver:
                beaverScript.FixedBeaverBehavior();
                break;
            
            case boss.Frog:
                frogScript.FixedFrogBehavior();
                break;
        }
    }


    public void TakeDamages(int damages, GameObject bullet)
    {
            switch (bossType)
            {
                case boss.Beaver:
                    beaverScript.TakeDamages(damages, bullet);
                    break;

                case boss.Frog:
                    frogScript.TakeDamages(damages, bullet);
                    break;

                case boss.Turtle:
                    turtleScript.TakeDamages(damages, bullet);
                    break;
            }
    }

    public void Death()
    {
        MapManager.Instance.activeRoom.GetComponent<DoorManager>().PortesActives();
        
        Destroy(gameObject);
    }
}