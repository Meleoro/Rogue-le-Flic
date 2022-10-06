using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletSpeed;
    public Vector2 directionBullet;
    
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = transform.right * bulletSpeed;
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
