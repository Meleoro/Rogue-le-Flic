using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider2D;
    [SerializeField] private EdgeCollider2D _edgeCollider2D;
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !ManagerChara.Instance.isFalling)
        {
            if (!ManagerChara.Instance.isDashing)
            {
                Vector2 newPos =  col.transform.position + (col.transform.position - transform.position).normalized * 2;

                StartCoroutine(ManagerChara.Instance.Fall(1, newPos));
            }
        }
        
        else if (col.CompareTag("Ennemy"))
        {
            col.GetComponent<Ennemy>().TakeDamages(1000, gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && !ManagerChara.Instance.isFalling)
        {
            if (!ManagerChara.Instance.isDashing)
            {
                Vector2 newPos =  col.transform.position + (col.transform.position - transform.position).normalized * 2;

                StartCoroutine(ManagerChara.Instance.Fall(1, newPos));
            }
        }
        
        else if (col.gameObject.CompareTag("Ennemy"))
        {
            StartCoroutine(col.gameObject.GetComponent<Ennemy>().Fall());
        }
    }


    private void Update()
    {
        if (ManagerChara.Instance.isDashing)
        {
            _edgeCollider2D.enabled = false;
        }

        else
        {
            _edgeCollider2D.enabled = true;
        }
    }
}
