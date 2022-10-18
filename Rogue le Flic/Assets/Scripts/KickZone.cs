using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class KickZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ennemy"))
        {
            KickChara.Instance.kickedEnnemy = col.gameObject;
            
            if (col.gameObject.GetComponent<Ennemy>().isCharging)
            {
                col.gameObject.GetComponent<Ennemy>().StopCoroutines();
                
                // EFFETS VISUELS
                KickChara.Instance.SlowMoStrong();
                KickChara.Instance.CameraShake();
                
                KickChara.Instance.AutoAim();
            }
            else
            {
                col.gameObject.GetComponent<Ennemy>().StopCoroutines();
                
                // EFFETS VISUELS
                KickChara.Instance.SlowMo();
                KickChara.Instance.CameraShake();
            }
            
            Vector2 direction = col.transform.position - transform.position;
            
            col.GetComponent<Rigidbody2D>().AddForce(direction.normalized * KickChara.Instance.kickStrenght, ForceMode2D.Impulse);
        }
    }
    
    
    
}
