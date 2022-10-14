using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Magnet: MonoBehaviour
{
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
