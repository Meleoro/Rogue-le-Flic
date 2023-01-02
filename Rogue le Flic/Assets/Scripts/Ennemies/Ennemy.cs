using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ennemy : MonoBehaviour
{
    public enum ennemies
    {
        Beaver,
        Frog,
        Turtle
    }

    public ennemies ennemyType;
    
    private Beaver beaverScript;
    private Frog frogScript;
    private Turtle turtleScript;

    [HideInInspector] public bool isCharging;
    [HideInInspector] public bool isDying;
    private bool isSpawning;

    [Header("Kicked")] 
    private float timerKick;

    [Header("Loot")] 
    [SerializeField] private int minCoins;
    [SerializeField] private int maxCoins;
    [SerializeField] private bool ammunitionActive;
    [Range(0, 100)] [SerializeField] private int probaAmmunition;
    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject ammunition;
 
    [Header("References")]
    public GameObject cible;
    public Animator anim;
    [SerializeField] private GameObject spawnIndicator;
    public GameObject sprite;


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

        if (timerKick > 0)
        {
            timerKick -= Time.deltaTime;

            if (timerKick <= 0)
            {
                switch (ennemyType)
                {
                    case ennemies.Beaver:
                        beaverScript.isKicked = false;
                        break;

                    case ennemies.Frog:
                        frogScript.isKicked = false;
                        break;

                    case ennemies.Turtle:
                        turtleScript.isKicked = false;
                        break;
                }
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
        if (!isDying)
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
    }

    public void isKicked()
    {
        switch (ennemyType)
        {
            case ennemies.Beaver:
                if(!beaverScript.isKicked)
                {
                    beaverScript.isKicked = true;
                    timerKick = 0.3f;
                }
                break;

            case ennemies.Frog:
                if (!frogScript.isKicked)
                {
                    frogScript.isKicked = true;
                    timerKick = 0.3f;
                }
                break;

            case ennemies.Turtle:
                if (!turtleScript.isKicked)
                {
                    turtleScript.isKicked = true;
                    timerKick = 0.3f;
                }
                break;
        }
    }

    public IEnumerator Death()
    {
        if (ammunitionActive)
        {
            int ammo = Random.Range(0, 100);

            if (ammo < probaAmmunition)
            {
                Instantiate(ammunition, transform.position, Quaternion.identity);
            }
            else
            {
                int coinNumber = Random.Range(minCoins, maxCoins + 1);

                for (int k = 0; k < coinNumber; k++)
                {
                    Instantiate(coin,transform.position, Quaternion.identity);
                }
            }
        }
        else
        {
            int coinNumber = Random.Range(minCoins, maxCoins + 1);

            for (int k = 0; k < coinNumber; k++)
            {
                Instantiate(coin,transform.position, Quaternion.identity);
            }
        }

        isDying = true;
        
        anim.SetTrigger("death");
        
        ScoreManager.instance.EnemyKilled();
        
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }


    public IEnumerator Fall()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        sprite.transform.DOScale(new Vector3(0, 0, 0), 0.5f);


        yield return new WaitForSeconds(0.5f);


        int coinNumber = Random.Range(minCoins, maxCoins + 1);

        for (int k = 0; k < coinNumber; k++)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
        }

        isDying = true;

        anim.SetTrigger("death");

        ScoreManager.instance.EnemyKilled();

        Destroy(gameObject);
    }


    public IEnumerator FinalDeath()
    {
        if (!GenerationPro.Instance.testLDMode)
        {
            if (!MapManager.Instance.activeRoom.GetComponent<DoorManager>().disableEndEffect)
            {
                ScoreManager.instance.EnemyKilled();
                
                isDying = true;

                transform.DOShakePosition(1, 1);
                MapManager.Instance.activeRoom.GetComponent<DoorManager>().isFinished = true;

                // CAMERA
                CameraMovements.Instance.endRoom = true;

                CameraMovements.Instance.timerZoom = 1f;
                CameraMovements.Instance.timeZoom = 1f;
                CameraMovements.Instance.ennemyPos = transform.position;

                yield return new WaitForSeconds(1);

                MapManager.Instance.activeRoom.GetComponent<DoorManager>().PortesActives();
                MapManager.Instance.activeRoom.GetComponent<DoorManager>().EndRoom(transform.position);
                

                Destroy(gameObject);
            }

            else
            {
                Destroy(gameObject);
            }
        }
    }


    private IEnumerator Spawn()
    {
            spawnIndicator.SetActive(true);
            sprite.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = false;

            isSpawning = true;

        spawnIndicator.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0);
        spawnIndicator.transform.DOScale(new Vector3(2f, 2f, 2f), 2);

        yield return new WaitForSeconds(2);
        
            spawnIndicator.SetActive(false);
            sprite.SetActive(true);
            GetComponent<BoxCollider2D>().enabled = true;
       
            isSpawning = false;
    }


    public void StopCoroutines()
    {
        isCharging = false;

        if (!isDying)
        {
            switch (ennemyType)
            {
                case ennemies.Beaver:
                    beaverScript.StopCoroutine();
                    break;

                case ennemies.Frog:
                    frogScript.StopCoroutine();
                    break;

                case ennemies.Turtle:
                    turtleScript.StopCoroutine();
                    break;
            }
        }
    }
}
