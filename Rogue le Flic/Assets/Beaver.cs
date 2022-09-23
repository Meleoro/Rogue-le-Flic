using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using DG.Tweening;

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
    private Rigidbody2D rb;
    private bool canMove;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = dragDeceleration * dragMultiplier;

        canMove = true;
    }


    private void Update()
    {
        if (health < 0)
        {
            Destroy(gameObject);
        }

        if (canMove)
        {
            Vector2 direction = AIPath.targetDirection;
            float distance = Mathf.Sqrt(Mathf.Pow(AIPath.destination.x - transform.position.x, 2) + 
                                        Mathf.Pow(AIPath.destination.y - transform.position.y, 2));

            if (distance < distanceJumpTrigger && !isJumping)
            {
                StartCoroutine(Jump(new Vector2(AIPath.destination.x - transform.position.x, AIPath.destination.y - transform.position.y)));
            }

            else
            {
                rb.AddForce(new Vector2(direction.x * speedX, direction.y* speedY), ForceMode2D.Force);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            health -= 1;
            Destroy(col.gameObject);

            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(col.gameObject.GetComponent<Rigidbody2D>().velocity.x / 2, 
                col.gameObject.GetComponent<Rigidbody2D>().velocity.y / 2), ForceMode2D.Impulse);
        }
    }
    

    IEnumerator Jump(Vector2 direction)
    {
        isJumping = true;
        canMove = false;
        
        transform.DOShakePosition(0.75f, 0.3f);
        
        yield return new WaitForSeconds(0.75f);

        rb.AddForce(direction.normalized * strenghtJump, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(0.25f);
        
        canMove = true;
        
        yield return new WaitForSeconds(3);
        
        isJumping = false;
    }
}
