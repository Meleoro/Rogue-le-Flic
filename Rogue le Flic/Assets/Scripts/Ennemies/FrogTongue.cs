using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Collections;

public class FrogTongue : MonoBehaviour
{
    public Vector2 destination;
    public Vector2 retour;
    public float tongueDuration;

    private float avancée;

    private Rigidbody2D rb;

    private Frog frog;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //Vector3 direction = ManagerChara.Instance.transform.position - transform.position;
        //destination = ManagerChara.Instance.transform.position + direction.normalized * 2;

        retour = transform.position;

        frog = GetComponentInParent<Frog>();
    }

    private void Update()
    {
        avancée += Time.deltaTime / frog.shotDuration;
        
        transform.position = new Vector2(Mathf.Lerp(retour.x, destination.x, frog.tonguePatern.Evaluate(avancée)),
            Mathf.Lerp(retour.y, destination.y, frog.tonguePatern.Evaluate(avancée)));

        if (avancée >= 1)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            HealthChara.instance.TakeDamage(30);
        }
    }
}
