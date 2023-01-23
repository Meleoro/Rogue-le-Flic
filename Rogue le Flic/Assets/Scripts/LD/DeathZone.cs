using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider2D;
    [SerializeField] private EdgeCollider2D _edgeCollider2D;

    public bool isInTuto;
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !ManagerChara.Instance.isFalling)
        {
            if (!ManagerChara.Instance.isDashing)
            {
                Vector2 newPos =  col.transform.position + (col.transform.position - transform.position).normalized * 2.5f;

                if(isInTuto)
                    newPos = transform.position - new Vector3(0, 3, 0);

                StartCoroutine(ManagerChara.Instance.Fall(1, newPos));
            }
        }
        
        /*else if (col.CompareTag("Ennemy"))
        {
            StartCoroutine(col.gameObject.GetComponent<Ennemy>().Fall());
        }*/
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && !ManagerChara.Instance.isFalling)
        {
            if (!ManagerChara.Instance.isDashing)
            {
                Vector2 newPos =  col.transform.position + (col.transform.position - transform.position).normalized * 2.5f;

                if (isInTuto)
                    newPos = transform.position - new Vector3(0, 3, 0);

                StartCoroutine(ManagerChara.Instance.Fall(1, newPos));
            }
        }
        
        else if (col.gameObject.CompareTag("Ennemy"))
        {
            if (!GenerationPro.Instance.testLDMode) 
            {
                MapManager.Instance.activeRoom.GetComponent<DoorManager>().ennemyCount -= 1;
            }

            if (MapManager.Instance.activeRoom.GetComponent<DoorManager>().ennemyCount <= 0)
            {
                StartCoroutine(col.gameObject.GetComponent<Ennemy>().FinalDeath());
            }
            else
            {
                Vector2 direction = col.contacts[0].point - (Vector2) col.transform.position;

                StartCoroutine(col.gameObject.GetComponent<Ennemy>().Fall(direction));
            }
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
