using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    void Update()
    { 
        void OnTriggerStay2D(Collider2D collision)
            {
                if (collision.CompareTag("Coin"))
                {
                    if (collision.gameObject.TryGetComponent(out Token coin))
                    {
                        coin.SetTarget(transform.parent.position);
                    }
                }
            }
    }
    
}
