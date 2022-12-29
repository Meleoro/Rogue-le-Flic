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
    private Frog frogScript;
    private Turtle turtleScript;

    [Header("References")]
    public Animator anim;
    public GameObject sprite;


    private void Start()
    {
        switch (bossType)
        {
            case boss.Beaver:
                beaverScript = GetComponent<BeaverBoss>();
                break;

            case boss.Frog:
                frogScript = GetComponent<Frog>();
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
        }
    }
}