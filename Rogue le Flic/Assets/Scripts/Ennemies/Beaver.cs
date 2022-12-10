using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.TextCore.Text;

public class Beaver : MonoBehaviour
{
    [Header("Movement Speeds")] 
    public float speedX;
    public float speedY;

    [Header("Deceleration")] 
    public float dragDeceleration;
    public float dragMultiplier;
    
    [Header("Castor")]
    [SerializeField] private int health;
    [SerializeField] private float distanceJumpTrigger;
    [SerializeField] private float strenghtJump;
    private bool isJumping;

    [Header("Autres")] 
    public AIPath AIPath;
    public AIDestinationSetter AIDestination;
    [SerializeField] private ParticleSystem hitEffect;
    public GameObject coins;
    private Rigidbody2D rb;
    private bool canMove;
    private Vector2 direction;
    private Ennemy ennemy;
    
    private bool stopDeath;
    [HideInInspector] public bool isKicked;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = dragDeceleration * dragMultiplier;

        canMove = true;
        
        AIDestination.target = ManagerChara.Instance.transform;

        ennemy = GetComponent<Ennemy>();
    }
    

    public void BeaverBehavior()
    {
        if (health <= 0 && !stopDeath)
        {
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
        
            if (distance < distanceJumpTrigger && !isJumping)
            {
                ennemy.anim.SetTrigger("isAttacking");

                StartCoroutine(Jump(new Vector2(AIPath.destination.x - transform.position.x, AIPath.destination.y - transform.position.y)));
            }

            // ROTATION CASTOR
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
    

    public void BeaverFixedBehavior()
    {
        if (canMove)
        {
            direction = AIPath.targetDirection;
            
            rb.AddForce(new Vector2(direction.x * speedX, direction.y * speedY) * 5, ForceMode2D.Force);

            ennemy.anim.SetBool("isWalking", true);
        }

        else
        {
            ennemy.anim.SetBool("isWalking", false);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Vector2 directionProj = col.transform.position - transform.position;
            
            HealthManager.Instance.LoseHealth(directionProj);
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


    public void TakeDamages(int damages, GameObject bullet)
    {
        health -= damages;

        // RECUL
        if(bullet.CompareTag("Bullet"))
            rb.AddForce(bullet.GetComponent<Bullet>().directionBullet * bullet.GetComponent<Bullet>().bulletKnockback, ForceMode2D.Impulse);

        else
        {
            Vector2 directionForce = new Vector2(transform.position.x - bullet.transform.position.x, transform.position.y - bullet.transform.position.y);
            
            StopCoroutine();
            
            rb.AddForce(directionForce.normalized * 10, ForceMode2D.Impulse);
        }

        hitEffect.Clear();
        hitEffect.Play();
    } 


    public void StopCoroutine()
    {
        if (!canMove)
        {
            StopAllCoroutines();
            canMove = true;
            transform.DOComplete();
                
            StartCoroutine(Wait());
        }
    }

    IEnumerator Jump(Vector2 directionJump)
    {
        isJumping = true;
        canMove = false;
        
        rb.AddForce(-directionJump.normalized * (strenghtJump / 5), ForceMode2D.Impulse);

        //transform.DOShakePosition(0.75f, 0.3f);

        GetComponent<Ennemy>().isCharging = true;
        
        yield return new WaitForSeconds(0.75f);

        rb.AddForce(directionJump.normalized * strenghtJump, ForceMode2D.Impulse);
        
        GetComponent<Ennemy>().isCharging = false;
        
        yield return new WaitForSeconds(0.25f);
        
        canMove = true;
        
        yield return new WaitForSeconds(3);

        isJumping = false;
    }
    

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);

        isJumping = false;
    }
}
