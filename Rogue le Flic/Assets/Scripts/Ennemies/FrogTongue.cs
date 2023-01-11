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
    private LineRenderer lr;

    private Frog frog;

    private EdgeCollider2D edgeCollider;
    private List<Vector2> edgeColliderPoints = new List<Vector2>();

    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        retour = transform.position;

        frog = GetComponentInParent<Frog>();
        
        lr.SetPosition(0, retour);
        lr.SetPosition(1, retour);

        edgeColliderPoints.Add(transform.localPosition);
        edgeColliderPoints.Add(transform.localPosition);
    }

    private void Update()
    {
        lr.SetPosition(1, transform.position);

        edgeColliderPoints[1] =  (-transform.position + frog.gameObject.transform.position) * 2;
        edgeCollider.SetPoints(edgeColliderPoints);

        avancée += Time.deltaTime / frog.frogData.shotDuration;
        
        transform.position = new Vector2(Mathf.Lerp(retour.x, destination.x, frog.frogData.tonguePatern.Evaluate(avancée)),
            Mathf.Lerp(retour.y, destination.y, frog.frogData.tonguePatern.Evaluate(avancée)));

        if (avancée >= 1)
        {
            Destroy(gameObject);
        }

        if (frog.stopTongue || frog.canMove)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Vector2 direction = col.transform.position - transform.position;
            
            HealthManager.Instance.LoseHealth(direction);
        }
    }
}
