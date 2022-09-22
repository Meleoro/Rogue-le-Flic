using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Beaver : MonoBehaviour
{
    [Header("Movement Speeds")] 
    public float speedX;
    public float speedY;

    [Header("Deceleration")] 
    public float dragDeceleration;
    public float dragMultiplier;
    
    [Header("Paramètres")]
    [SerializeField] private int health;

    [Header("Autres")] 
    public AIPath AIPath;
    private Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = dragDeceleration * dragMultiplier;
    }


    private void Update()
    {
        if (health < 0)
        {
            Destroy(gameObject);
        } 

        Vector2 destination = AIPath.targetDirection;
        rb.AddForce(new Vector2(destination.x * speedX, destination.y* speedY), ForceMode2D.Force);
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
}
