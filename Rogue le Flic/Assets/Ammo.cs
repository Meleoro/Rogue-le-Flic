using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            ManagerChara.Instance.activeGun.GetComponent<Gun>().AddAmmo(ManagerChara.Instance.activeGun.GetComponent<Gun>().gunData.maxAmmo);
            
            Destroy(gameObject);
        }
    }
}
