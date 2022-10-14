using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float radius;
    public CircleCollider2D magnet;

    private void Update()
    {
        
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Ennemy"))
        {
            if (collision.gameObject.TryGetComponent(out EXP exp))
            {
                exp.SetTarget(transform.parent.position);
            }
        }
    }
}
