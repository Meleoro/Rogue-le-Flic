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
    private bool canShake;
    private bool lookLeft;


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
        
        canShake = false;
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
        else if(canShake)
        {
            canShake = false;
            transform.DOShakePosition(0.12f, 0.2f).OnComplete(() => canShake = true);
        }

        if (ReferenceChoice.Instance.kicked || ReferenceChoice.Instance.spared)
        {
            StartCoroutine(EndCinematicDeath());
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
        if (!death)
        {
            death = true;
            StartCoroutine(CinematicDeath());
        }
    }


    IEnumerator CinematicDeath()
    {
        ManagerChara.Instance.savePosition = ManagerChara.Instance.transform.position;

        ReferenceCamera.Instance.fondNoir.DOFade(1, 2);
        ReferenceCamera.Instance.finalCinematic = true;

        ManagerChara.Instance.noControl = true;

        CameraMovements.Instance.bossEndRoom = true;
        CameraMovements.Instance.timeZoom = 2;

        switch (bossType)
        {
            case boss.Beaver:
                if (!beaverScript.lookLeft)
                {
                    CameraMovements.Instance.posCamera = transform.position + new Vector3(4.5f, 0, 0);
                    lookLeft = false;
                }
                else
                {
                    CameraMovements.Instance.posCamera = transform.position + new Vector3(-4.5f, 0, 0);
                    lookLeft = true;
                }
                break;

            case boss.Frog:
                if (!frogScript.lookLeft)
                {
                    CameraMovements.Instance.posCamera = transform.position + new Vector3(4.5f, 0, 0);
                    lookLeft = false;
                }
                else
                {
                    CameraMovements.Instance.posCamera = transform.position + new Vector3(-4.5f, 0, 0);
                    lookLeft = true;
                }
                break;

            case boss.Turtle:
                if (!turtleScript.lookLeft)
                {
                    CameraMovements.Instance.posCamera = transform.position + new Vector3(4.5f, 0, 0);
                    lookLeft = false;
                }
                else
                {
                    CameraMovements.Instance.posCamera = transform.position + new Vector3(-4.5f, 0, 0);
                    lookLeft = true;
                }
                break;
        }

        yield return new WaitForSeconds(2);


        transform.position = transform.position + new Vector3(0, -5000, 0);
        CameraMovements.Instance.transform.position = CameraMovements.Instance.transform.position + new Vector3(0, -5000, 0);

        ReferenceCamera.Instance.finalCinematicChara = true;

        if (lookLeft)
        {
            ManagerChara.Instance.transform.position = transform.position + new Vector3(-20, 0, 0);

            ManagerChara.Instance.transform.DOMoveX(transform.position.x - 10, 2).SetEase(Ease.Linear);
        }

        else
        {
            Debug.Log(transform.position);
            
            ManagerChara.Instance.transform.position = transform.position + new Vector3(20, 0, 0);

            ManagerChara.Instance.transform.DOMoveX(transform.position.x + 10, 2).SetEase(Ease.Linear);
        }

        canShake = true;

        yield return new WaitForSeconds(2);


        ReferenceChoice.Instance.kick.DOLocalMoveX(400, 2);
        ReferenceChoice.Instance.spare.DOLocalMoveX(-400, 2);

        yield return new WaitForSeconds(2);


        ReferenceChoice.Instance.kick.GetComponent<Animator>().enabled = true;
        ReferenceChoice.Instance.spare.GetComponent<Animator>().enabled = true;
    }


    public IEnumerator EndCinematicDeath()
    {
        ReferenceChoice.Instance.kick.GetComponent<Animator>().enabled = false;
        ReferenceChoice.Instance.spare.GetComponent<Animator>().enabled = false;

        ManagerChara.Instance.transform.position = ManagerChara.Instance.savePosition;
        CameraMovements.Instance.transform.position = ManagerChara.Instance.savePosition;

        ReferenceChoice.Instance.kick.DOLocalMoveX(800, 1);
        ReferenceChoice.Instance.spare.DOLocalMoveX(-800, 1);

        ReferenceCamera.Instance.fondNoir.DOFade(0, 1);

        yield return new WaitForSeconds(1);


        switch (bossType)
        {
            case boss.Beaver:
                LevelManager.Instance.banishedRooms.Add(0);
                break;

            case boss.Frog:
                LevelManager.Instance.banishedRooms.Add(1);
                break;

            case boss.Turtle:
                LevelManager.Instance.banishedRooms.Add(2);
                break;
        }

        ManagerChara.Instance.noControl = false;
        CameraMovements.Instance.bossEndRoom = false;

        MapManager.Instance.activeRoom.GetComponent<DoorManager>().PortesActives();

        Destroy(gameObject);
    }
}