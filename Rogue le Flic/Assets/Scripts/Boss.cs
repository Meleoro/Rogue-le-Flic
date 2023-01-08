using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    private TurtleBoss turtleScript;

    private bool death;

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
                turtleScript = GetComponent<TurtleBoss>();
                break;
        }
    }


    private void Update()
    {
        if (!death)
        {
            switch (bossType)
            {
                case boss.Beaver:
                    beaverScript.BeaverBehavior();
                    break;

                case boss.Frog:
                    frogScript.FrogBehavior();
                    break;

                case boss.Turtle:
                    turtleScript.TurtleBehavior();
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!death)
        {
            switch (bossType)
            {
                case boss.Beaver:
                    beaverScript.FixedBeaverBehavior();
                    break;

                case boss.Frog:
                    frogScript.FixedFrogBehavior();
                    break;

                case boss.Turtle:
                    turtleScript.FixedTurtleBehavior();
                    break;
            }
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
        death = true;

        StartCoroutine(CinematicDeath());
    }


    IEnumerator CinematicDeath()
    {
        ReferenceCamera.Instance.fondNoir.GetComponent<SpriteRenderer>().DOFade(1, 2);
        ReferenceCamera.Instance.finalCinematic = true;

        CameraMovements.Instance.bossEndRoom = true;
        CameraMovements.Instance.timeZoom = 1;
        CameraMovements.Instance.timerZoom = 1;

        CameraMovements.Instance.posCamera = transform.position + new Vector3(-5, 0, 0);


        yield return new WaitForSeconds(4);

        MapManager.Instance.activeRoom.GetComponent<DoorManager>().PortesActives();

        Destroy(gameObject);
    }
}