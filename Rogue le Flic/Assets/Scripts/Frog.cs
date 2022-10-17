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
    [SerializeField] private float shotDuration;
    private bool cooldownShot;

    [Header("Autres")]
    public AIPath AIPath;
    public AIDestinationSetter AIDestination;
    private Rigidbody2D rb;
    private bool canMove;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = dragDeceleration * dragMultiplier;

        canMove = true;
        
        AIDestination.target = ManagerChara.Instance.transform;
    }
    

    public void FrogBehavior()
    {
        if (health < 0)
        {
            Destroy(gameObject);
        }
        
        float distance = Mathf.Sqrt(Mathf.Pow(AIPath.destination.x - transform.position.x, 2) + 
                                    Mathf.Pow(AIPath.destination.y - transform.position.y, 2));
        
        if (distance <= distanceShotTrigger && !cooldownShot)
        {
            Shoot();

            StartCoroutine(Cooldown());
        }
    }

    public void FrogFixedBehavior()
    {
        if (canMove)
        {
            Vector2 direction = AIPath.targetDirection;

            rb.AddForce(new Vector2(direction.x * speedX, direction.y * speedY) * 5, ForceMode2D.Force);
        }
    }


    void Shoot()
    {
        GameObject currentTongue = Instantiate(tongue, transform.position, Quaternion.identity);

        currentTongue.GetComponent<FrogTongue>().tongueDuration = shotDuration;
    }

    IEnumerator Cooldown()
    {
        canMove = false;
        cooldownShot = true;
        
        yield return new WaitForSeconds(shotDuration);

        canMove = true;
        
        yield return new WaitForSeconds(shotDuration / 2);

        cooldownShot = false;
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            health -= col.gameObject.GetComponent<Bullet>().bulletDamages;
            Destroy(col.gameObject);

            rb.velocity = Vector2.zero;

            // RECUL
            rb.AddForce(col.gameObject.GetComponent<Bullet>().directionBullet * col.gameObject.GetComponent<Bullet>().bulletKnockback, ForceMode2D.Impulse);
        }
        if (col.gameObject.CompareTag("Player"))
        {
            HealthChara.instance.TakeDamage(15);
        }
    }
}
