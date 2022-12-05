using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pathfinding;

public class Frog : MonoBehaviour
{
    [Header("Movement Speeds")] 
    public float speedX;
    public float speedY;

    [Header("Deceleration")] 
    public float dragDeceleration;
    public float dragMultiplier;
    
    [Header("Frog")]
    [SerializeField] private int health;
    public GameObject tongue;
    [SerializeField] private float distanceShotTrigger;
    public float shotDuration;
    public AnimationCurve tonguePatern;
    private bool cooldownShot;
    
    [SerializeField] private Vector2 posLeft;
    [SerializeField] private Vector2 posRight;
    private Vector2 direction;
    private bool lookLeft;
    private Ennemy ennemy;

    [Header("Autres")]
    public AIPath AIPath;
    public AIDestinationSetter AIDestination;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private GameObject coins;
    private Rigidbody2D rb;
    [HideInInspector] public bool canMove;

    private bool stopDeath;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = dragDeceleration * dragMultiplier;

        canMove = true;
        
        AIDestination.target = ManagerChara.Instance.transform;

        ennemy = GetComponent<Ennemy>();
    }
    

    public void FrogBehavior()
    {
        if (health <= 0 && !stopDeath)
        {
            stopDeath = true;
            
            MapManager.Instance.activeRoom.GetComponent<DoorManager>().ennemyCount -= 1;

            if (MapManager.Instance.activeRoom.GetComponent<DoorManager>().ennemyCount <= 0 && !GenerationPro.Instance.testLDMode)
            {
                StartCoroutine(ennemy.FinalDeath());
            }
            else
            {
                Instantiate(coins,transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        
        float distance = Mathf.Sqrt(Mathf.Pow(AIPath.destination.x - transform.position.x, 2) + 
                                    Mathf.Pow(AIPath.destination.y - transform.position.y, 2));
        
        if (distance <= distanceShotTrigger && !cooldownShot)
        {
            ennemy.anim.SetTrigger("isAttacking");
            
            StartCoroutine(Cooldown());
        }
        
        // ROTATION FROG
        if (direction.x > 0.1f)
        {
            lookLeft = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
        else if (direction.x < -0.1f)
        {
            lookLeft = true;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (lookLeft)
        {
            ennemy.sprite.transform.localPosition = posLeft;
        }
        else
        {
            ennemy.sprite.transform.localPosition = posRight;
        }
    }

    public void FrogFixedBehavior()
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
    

    IEnumerator Cooldown()
    {
        canMove = false;
        cooldownShot = true;
        
        Vector3 direction = ManagerChara.Instance.transform.position - transform.position;
        Vector2 destination = ManagerChara.Instance.transform.position + direction.normalized * 3;
        
        transform.DOShakePosition(0.75f, 0.3f);

        GetComponent<Ennemy>().isCharging = true;
        
        yield return new WaitForSeconds(0.75f);
        
        GetComponent<Ennemy>().isCharging = false;
        
        GameObject currentTongue = Instantiate(tongue, transform.position, Quaternion.identity, transform);

        currentTongue.GetComponent<FrogTongue>().destination = destination;
        currentTongue.GetComponent<FrogTongue>().tongueDuration = shotDuration;

        yield return new WaitForSeconds(shotDuration);

        canMove = true;
        
        rb.AddForce(-direction * 3, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(shotDuration / 2);

        cooldownShot = false;
    }
    
    
    public void StopCoroutine()
    {
        if (!canMove)
        {
            StopAllCoroutines();
            canMove = true;
            transform.DOComplete();
                
            StartCoroutine(Wait(4));
        }
    }

    IEnumerator Wait(float duree)
    {
        cooldownShot = true;
        
        yield return new WaitForSeconds(duree);
        
        cooldownShot = false;
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Vector2 direction = col.transform.position - transform.position;
            
            HealthManager.Instance.LoseHealth(direction);
        }
    }

    public void TakeDamages(int damages, GameObject bullet)
    {
        health -= damages;

        rb.velocity = Vector2.zero;

        if (canMove)
        {
            // RECUL
            rb.AddForce(bullet.GetComponent<Bullet>().directionBullet * bullet.GetComponent<Bullet>().bulletKnockback, ForceMode2D.Impulse);
        }

        // SI LA LANGUE EST ACTUELLEMENT LANCEE 
        else
        {
            // SHAKE
            gameObject.transform.DOShakePosition(0.1f, 0.1f);
        }

        hitEffect.Clear();
        hitEffect.Play();
    }
}
