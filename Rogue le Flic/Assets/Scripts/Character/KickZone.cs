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
            if (!col.gameObject.GetComponent<Ennemy>().isDying)
            {
                KickChara.Instance.kickedEnnemy = col.gameObject;

                /*
                if (col.gameObject.GetComponent<Ennemy>().isCharging)
                {
                    col.gameObject.GetComponent<Ennemy>().StopCoroutines();

                    // EFFETS VISUELS
                    KickChara.Instance.SlowMoStrong();
                    KickChara.Instance.CameraShake();

                    KickChara.Instance.AutoAim();
                }*/

                if (col.gameObject.GetComponent<Ennemy>().ennemyType == Ennemy.ennemies.Turtle)
                {
                    if (col.gameObject.GetComponent<Turtle>().isSliding)
                    {
                        col.gameObject.GetComponent<Turtle>().Kicked(-ManagerChara.Instance.transform.position + col.transform.position);
                    }

                    else
                    {
                        col.gameObject.GetComponent<Ennemy>().StopCoroutines();

                        col.gameObject.GetComponent<Ennemy>().isKicked();
                    }

                    KickChara.Instance.SlowMo(1.5f);
                    KickChara.Instance.CameraShake();
                }

                else
                {
                    col.gameObject.GetComponent<Ennemy>().StopCoroutines();

                    // EFFETS VISUELS
                    KickChara.Instance.SlowMo(0.9f);
                    KickChara.Instance.CameraShake();

                    col.gameObject.GetComponent<Ennemy>().isKicked();
                }

                Vector2 direction = col.transform.position - transform.position;

                col.GetComponent<Rigidbody2D>().AddForce(direction.normalized * KickChara.Instance.kickStrenght, ForceMode2D.Impulse);
            }
        }

        else if (col.CompareTag("Boss"))
        {
            KickChara.Instance.kickedEnnemy = col.gameObject;

            if (col.gameObject.GetComponent<Boss>().bossType == Boss.boss.Turtle)
            {
                if (col.gameObject.GetComponent<TurtleBoss>().isSliding)
                {
                    col.gameObject.GetComponent<TurtleBoss>().Kicked(-ManagerChara.Instance.transform.position + col.transform.position);
                }
            }

            KickChara.Instance.SlowMo(0.9f);
            KickChara.Instance.CameraShake();


            Vector2 direction = col.transform.position - transform.position;

            col.GetComponent<Rigidbody2D>().AddForce(direction.normalized * KickChara.Instance.kickStrenght / 2, ForceMode2D.Impulse);
        }
        
        else if (col.CompareTag("Box"))
        {
            col.GetComponent<Box>().Kicked(-KickChara.Instance.kickDirection);

            KickChara.Instance.SlowMo(2);
            KickChara.Instance.CameraShake();
        }
    }
    
    
    
}
