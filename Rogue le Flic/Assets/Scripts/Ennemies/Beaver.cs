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
    public BeaverData niveau1;
    public BeaverData niveau2;
    public BeaverData niveau3;

    private BeaverData beaverData;

    private int currentHealth;
    
    
    [Header("Castor")]
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

    private float stunTimer;
    
    public bool isInTuto;

    public AudioSource windUp;
    
    

    private void Start()
    {
        if (LevelManager.Instance.currentLevel == 1)
        {
            beaverData = niveau1;
        }
        
        else if (LevelManager.Instance.currentLevel == 2)
        {
            beaverData = niveau2;
        }
        
        else if (LevelManager.Instance.currentLevel == 3)
        {
            beaverData = niveau3;
        }

        else
        {
            beaverData = niveau1;
        }

        currentHealth = beaverData.health;

        rb = GetComponent<Rigidbody2D>();
        rb.drag = beaverData.dragDeceleration * beaverData.dragMultiplier;

        canMove = true;
        
        AIDestination.target = ManagerChara.Instance.transform;

        ennemy = GetComponent<Ennemy>();
    }
    

    public void BeaverBehavior()
    {
        if (currentHealth <= 0 && !stopDeath)
        {
            stopDeath = true;
            isKicked = false;

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
        
            if (distance < beaverData.distanceJumpTrigger && !isJumping)
            {
                ennemy.anim.SetTrigger("isAttacking");

                //here
                //windUp.Play();
                
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
            
            rb.AddForce(new Vector2(direction.x * beaverData.speedX, direction.y * beaverData.speedY) * 5, ForceMode2D.Force);

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
            col.GetComponent<Ennemy>().TakeDamages(DegatsManager.Instance.degatsKickedEnnemy, gameObject);
        }
        
        else if (isKicked && (col.CompareTag("Wall") || col.CompareTag("Box")))
        {
            if(col.CompareTag("Box"))
                col.GetComponent<Box>().Explose();
            
            TakeDamages(DegatsManager.Instance.degatsEnnemyIntoWall, gameObject);
            ennemy.Stun();
        }
    }


    public void TakeDamages(int damages, GameObject bullet)
    {
        if (bullet.CompareTag("Kick"))
        {
            currentHealth -= damages;
        }

        // RECUL
        else if (bullet.CompareTag("Bullet") && !isInTuto)
        {
            currentHealth -= damages;
            rb.AddForce(bullet.GetComponent<Bullet>().directionBullet * bullet.GetComponent<Bullet>().bulletKnockback, ForceMode2D.Impulse);
        }
        else if (bullet.CompareTag("Bullet") && isInTuto)
        {
            currentHealth -= damages * 3;
            rb.velocity = Vector2.zero;
        }

        else if(!isInTuto)
        {
            currentHealth -= damages;
            
            Vector2 directionForce = new Vector2(transform.position.x - bullet.transform.position.x, transform.position.y - bullet.transform.position.y);
            
            StopCoroutine();
            
            rb.AddForce(directionForce.normalized * 10, ForceMode2D.Impulse);
        }

        VerifyDeath();

        hitEffect.Clear();
        hitEffect.Play();
    } 
    
    
    public void VerifyDeath()
    {
        if (currentHealth <= 0 && !stopDeath)
        {
            stopDeath = true;
            isKicked = false;

            GetComponent<BoxCollider2D>().enabled = false;
            
            StopCoroutine();
            canMove = false;
            
            if (!GenerationPro.Instance.testLDMode) 
            {
                MapManager.Instance.activeRoom.GetComponent<DoorManager>().ennemyCount -= 1;
            }

            if (!GenerationPro.Instance.testLDMode)
            {
                if(MapManager.Instance.activeRoom.GetComponent<DoorManager>().ennemyCount <= 0 && !MapManager.Instance.activeRoom.GetComponent<DoorManager>().disableEndEffect)
                    StartCoroutine(ennemy.FinalDeath());
                
                                
                else
                    StartCoroutine(ennemy.Death());
            }
            else
            {
                StartCoroutine(ennemy.Death());
            }
        }
    }


    public void StopCoroutine()
    {
        if (!canMove && !ennemy.isDying)
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
        
        rb.AddForce(-directionJump.normalized * (beaverData.strenghtJump / 5), ForceMode2D.Impulse);

        //transform.DOShakePosition(0.75f, 0.3f);

        GetComponent<Ennemy>().isCharging = true;
        
        windUp.Play();

        yield return new WaitForSeconds(0.75f);

        rb.AddForce(directionJump.normalized * beaverData.strenghtJump, ForceMode2D.Impulse);
        
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
