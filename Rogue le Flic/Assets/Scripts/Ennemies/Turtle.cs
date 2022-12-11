using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;

public class Turtle : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float speedX;
    public float speedY;

    [Header("Deceleration")]
    public float dragDeceleration;
    public float dragMultiplier;

    [Header("Turtle")]
    [SerializeField] private int health;
    [SerializeField] private float distanceSlideTrigger;
    [SerializeField] private float speedSlide;
    [SerializeField] private int nbrRebonds;
    [SerializeField] private float cooldown;
    [HideInInspector] public bool isSliding;

    [Header("Autres")]
    public AIPath AIPath;
    public AIDestinationSetter AIDestination;
    [SerializeField] private ParticleSystem hitEffect;
    public GameObject coins;
    private Rigidbody2D rb;
    private bool canMove;
    private float timerCooldown;

    private Vector2 slideDirection;
    private int currentNbrRebonds;
    [HideInInspector] public bool isKicked;
    private Ennemy ennemy;

    private bool stopDeath;
    private Vector2 direction;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = dragDeceleration * dragMultiplier;

        canMove = true;

        AIDestination.target = ManagerChara.Instance.transform;
        
        timerCooldown = cooldown;

        ennemy = GetComponent<Ennemy>();
    }


    public void TurtleBehavior()
    {
        if (health <= 0 && !stopDeath)
        {
            isKicked = false;
            stopDeath = true;
            
            GetComponent<BoxCollider2D>().enabled = false;

            StopCoroutine();
            canMove = false;
            
            if (!GenerationPro.Instance.testLDMode) 
            {
                MapManager.Instance.activeRoom.GetComponent<DoorManager>().ennemyCount -= 1;
            }

            if (!GenerationPro.Instance.testLDMode)
            {
                if(MapManager.Instance.activeRoom.GetComponent<DoorManager>().ennemyCount <= 0)
                    StartCoroutine(ennemy.FinalDeath());

                else
                    StartCoroutine(ennemy.Death());
            }
            else
            {
                StartCoroutine(ennemy.Death());
            }
        }

        else
        {
            float distance = Mathf.Sqrt(Mathf.Pow(AIPath.destination.x - transform.position.x, 2) +
                                        Mathf.Pow(AIPath.destination.y - transform.position.y, 2));

            if (!isSliding && canMove)
            {
                if (timerCooldown <= 0 && distance < distanceSlideTrigger)
                {
                    StartCoroutine(Slide(new Vector2(AIPath.destination.x - transform.position.x, AIPath.destination.y - transform.position.y)));
                }
            
                else
                {
                    timerCooldown -= Time.deltaTime;
                }
            }


            // ROTATION TURTLE
            if (direction.x > 0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            else if (direction.x < -0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }


    public void TurtleFixedBehavior()
    {
        if (canMove)
        {
            direction = AIPath.targetDirection;

            rb.AddForce(new Vector2(direction.x * speedX, direction.y * speedY) * 5, ForceMode2D.Force);
        }

        else if (isSliding)
        {
            if(!isKicked)
                rb.AddForce(slideDirection * speedSlide, ForceMode2D.Force);
            
            else
                rb.AddForce(slideDirection * speedSlide * 1.5f, ForceMode2D.Force);
        }
    }

    
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Ennemy") && isSliding)
        {
            currentNbrRebonds += 1;

            slideDirection = Vector3.Reflect(slideDirection.normalized, col.contacts[0].normal);

            if (isKicked)
            {
                TakeDamages(3, col.gameObject);
                ennemy.anim.SetTrigger("reset");
            }

            // ON STOP SON COMPORTEMENT DE SLIDE
            if(currentNbrRebonds >= nbrRebonds)
            {
                isSliding = false;
                isKicked = false;
                canMove = true;

                timerCooldown = cooldown;

                currentNbrRebonds = 0;

                ennemy.anim.SetBool("isWalking", true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Vector2 direction = col.transform.position - transform.position;

            HealthManager.Instance.LoseHealth(direction);
        }
        
        else if (col.gameObject.CompareTag("Ennemy") && isSliding)
        {
            if(!isKicked)
                col.gameObject.GetComponent<Ennemy>().TakeDamages(1, gameObject);
            
            else
                col.gameObject.GetComponent<Ennemy>().TakeDamages(5, gameObject);
        }
        
        else if (col.gameObject.CompareTag("Box") && isSliding)
        {
            col.gameObject.GetComponent<Box>().Explose();
        }
        
        else if (isKicked && col.CompareTag("Ennemy"))
        {
            col.GetComponent<Ennemy>().TakeDamages(2, gameObject);
        }
        
        else if (isKicked)
        {
            TakeDamages(2, gameObject);
        }
    }


    public void TakeDamages(int damages, GameObject collider)
    {
        health -= damages;

        // RECUL
        if(collider.CompareTag("Bullet")) 
            rb.AddForce(collider.GetComponent<Bullet>().directionBullet * collider.GetComponent<Bullet>().bulletKnockback, ForceMode2D.Impulse);

        else
        {
            Vector2 directionForce = new Vector2(transform.position.x - collider.transform.position.x, transform.position.y - collider.transform.position.y);
            
            StopCoroutine();
            
            rb.AddForce(directionForce.normalized * 10, ForceMode2D.Impulse);
        }

        hitEffect.Clear();
        hitEffect.Play();
    }

    public void Kicked(Vector2 direction)
    {
        slideDirection = direction;
        isKicked = true;

        currentNbrRebonds = nbrRebonds;
    }


    public void StopCoroutine()
    {
        if (!canMove)
        {
            StopAllCoroutines();
            canMove = true;
            transform.DOComplete();

            isSliding = false;
            isKicked = false;
            timerCooldown = cooldown;
            
            if(!stopDeath)
                ennemy.anim.SetTrigger("reset");
        }
    }

    IEnumerator Slide(Vector2 direction)
    {
        canMove = false;

        ennemy.anim.SetTrigger("StartAttack");
        ennemy.anim.SetBool("isWalking", false);

        transform.DOShakePosition(0.75f, 0.3f);

        GetComponent<Ennemy>().isCharging = true;

        slideDirection = direction.normalized;

        yield return new WaitForSeconds(0.75f);


        //rb.AddForce(direction.normalized * speedSlide, ForceMode2D.Impulse);

        GetComponent<Ennemy>().isCharging = false;

        isSliding = true;
    }
}
