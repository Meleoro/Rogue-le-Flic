using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KickZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ennemy"))
        {
            col.gameObject.GetComponent<Beaver>().StopCoroutine();
            
            Vector2 direction = col.transform.position - transform.position;
            
            col.GetComponent<Rigidbody2D>().AddForce(direction.normalized * KickChara.Instance.kickStrenght, ForceMode2D.Impulse);
        }
    }
}
