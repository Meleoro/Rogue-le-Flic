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

    [Header("Bubble")] 
    public bool isBubble;
    public float decelerationBubble;
    public float timeExplosion;
    public float reachExplosion;
    
    
    [Header("Autres")]
    [HideInInspector] public Vector2 directionBullet;
    private Rigidbody2D rb;
    private float timerBubble;
    
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        timerBubble = timeExplosion;
    }

    void Update()
    {
        rb.velocity = transform.right * bulletSpeed;

        if (isBubble)
        {
            bulletSpeed -= Time.deltaTime * decelerationBubble;
            timerBubble -= Time.deltaTime;

            if (timerBubble <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void LateUpdate()
    {
        directionBullet = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
