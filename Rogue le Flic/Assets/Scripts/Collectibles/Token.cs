using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Token : MonoBehaviour
{
    //public AudioClip sound;
    public int currentTokenCount;
    bool hasTarget;
    Vector3 targetPosition;
    private float moveSpeed = 10f;
    private Rigidbody2D rb;
    
    private void Start()
    {
        currentTokenCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ChangeNumber());
            //currentTokenCount = currentTokenCount + 1;
            //GetComponent<SpriteRenderer>().enabled = false;
            //GetComponent<BoxCollider2D>().enabled = false;
            //AudioManager.instance.PlayClipAt(sound, transform.position);
        }
    }
    IEnumerator ChangeNumber()
    {
        yield return new WaitForSeconds(0.01f);
        CounterToken.instance.AddCounterToken(1);
        Destroy(gameObject,0.00001f);
    }
    void FixedUpdate()
    {
        if (hasTarget)
        {
            Vector2 targetDirection = targetPosition - transform.position;
            rb.velocity = targetDirection.normalized * moveSpeed;
        }
    }

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTarget = true;
    }
}
