using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private float magnetStrenght;
    
    void OnTriggerStay2D(Collider2D collision)
    { 
        if (collision.CompareTag("Coin"))
        {
            Vector2 direction = transform.position - collision.transform.position;

            collision.GetComponent<Coin>().isMagneted = true;
            collision.GetComponent<Rigidbody2D>().AddForce(direction.normalized * magnetStrenght, ForceMode2D.Force);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            other.GetComponent<Coin>().isMagneted = false;
        }
    }
}
