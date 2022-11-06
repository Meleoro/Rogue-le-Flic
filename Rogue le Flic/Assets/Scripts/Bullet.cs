using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("General")]
    public float bulletSpeed;
    public float bulletKnockback;
    public int bulletDamages;
    private Vector2 direction;

    [Header("Bubble")] 
    public bool isBubble;
    public float decelerationBubble;
    public float timeExplosion;
    [SerializeField] private GameObject bubble;
    [SerializeField] private GameObject bubbleExplosion;
    private float timerBubble;

    [Header("Modules")]
    [HideInInspector] public bool percante;
    [HideInInspector] public bool rebondissante;

    [Header("Autres")]
    [SerializeField] private BoxCollider2D bounceWalls;
    [HideInInspector] public Vector2 directionBullet;
    private Rigidbody2D rb;
    
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        timerBubble = timeExplosion;

        direction = transform.right;
    }

    void Update()
    {
        rb.velocity = direction * bulletSpeed;

        // BULLE
        if (isBubble)
        {
            timerBubble -= Time.deltaTime;
            
            if (timerBubble <= 0)
            {
                bubble.SetActive(false);
                bubbleExplosion.SetActive(true);
            }
            else
            {
                bulletSpeed -= Time.deltaTime * decelerationBubble;
            }

            if (timerBubble <= -0.5f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void LateUpdate()
    {
        directionBullet = rb.velocity;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ennemy"))
        {
            collision.GetComponent<Ennemy>().TakeDamages(bulletDamages, gameObject);

            if (!percante)
                Destroy(gameObject);

            bounceWalls.enabled = false;
        }

        else
        {
            if (!rebondissante)
                Destroy(gameObject);

            else
                bounceWalls.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        direction = Vector3.Reflect(direction.normalized, collision.contacts[0].normal);

        bounceWalls.enabled = false;
    }
}

