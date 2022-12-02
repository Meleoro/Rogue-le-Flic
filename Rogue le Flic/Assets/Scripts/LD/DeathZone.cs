using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
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
            Destroy(col);
        }
    }
}
