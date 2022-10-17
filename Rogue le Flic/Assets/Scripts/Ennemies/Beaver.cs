using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using DG.Tweening;
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
    public GameObject coins;
    public Transform beaverPos;
    private Rigidbody2D rb;
    private bool canMove;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = dragDeceleration * dragMultiplier;

        canMove = true;
        
        AIDestination.target = ManagerChara.Instance.transform;
    }
    

    public void BeaverBehavior()
    {
        if (health < 0)
        {
            Instantiate(coins,beaverPos.position, Quaternion.identity);
            Destroy(gameObject);
            ScoreManager.instance.AddPoint();
        }
        
        float distance = Mathf.Sqrt(Mathf.Pow(AIPath.destination.x - transform.position.x, 2) + 
                                    Mathf.Pow(AIPath.destination.y - transform.position.y, 2));
        
        if (distance < distanceJumpTrigger && !isJumping)
        {
            StartCoroutine(Jump(new Vector2(AIPath.destination.x - transform.position.x, AIPath.destination.y - transform.position.y)));
        }
    }
    

    public void BeaverFixedBehavior()
    {
        if (canMove)
        {
            Vector2 direction = AIPath.targetDirection;
            
            rb.AddForce(new Vector2(direction.x * speedX, direction.y * speedY) * 5, ForceMode2D.Force);
        }
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

    IEnumerator Jump(Vector2 direction)
    {
        isJumping = true;
        canMove = false;
        
        transform.DOShakePosition(0.75f, 0.3f);

        GetComponent<Ennemy>().isCharging = true;
        
        yield return new WaitForSeconds(0.75f);

        rb.AddForce(direction.normalized * strenghtJump, ForceMode2D.Impulse);
        
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
