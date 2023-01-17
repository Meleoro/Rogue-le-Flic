using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Collections;


public class BossFrogTongue : MonoBehaviour
{
    public Vector2 destination;
    public Vector2 retour;
    public float tongueDuration;

    private float avancée;

    private Rigidbody2D rb;
    private LineRenderer lr;

    private FrogBoss frog;

    private EdgeCollider2D edgeCollider;
    private List<Vector2> edgeColliderPoints = new List<Vector2>();

    private GameObject box;
    private bool boxStuck;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        retour = transform.position;

        frog = GetComponentInParent<FrogBoss>();

        lr.SetPosition(0, retour);
        lr.SetPosition(1, retour);

        edgeColliderPoints.Add(transform.localPosition);
        edgeColliderPoints.Add(transform.localPosition);
    }

    private void Update()
    {
        lr.SetPosition(1, transform.position);

        edgeColliderPoints[1] = (-transform.position + frog.gameObject.transform.position) * 2;
        edgeCollider.SetPoints(edgeColliderPoints);

        avancée += Time.deltaTime / frog.bossData.shotDuration;

        transform.position = new Vector2(Mathf.Lerp(retour.x, destination.x, frog.bossData.tonguePatern.Evaluate(avancée)),
            Mathf.Lerp(retour.y, destination.y, frog.bossData.tonguePatern.Evaluate(avancée)));

        if (avancée >= 1)
        {
            Destroy(gameObject);

            if (boxStuck)
            {
                Destroy(box);
                frog.Stun();
            }
        }

        else if (boxStuck)
        {
            box.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Vector2 direction = col.transform.position - transform.position;

            HealthManager.Instance.LoseHealth(direction);
        }

        if (col.gameObject.CompareTag("Box") && !boxStuck)
        { 
            box = col.gameObject;
            boxStuck = true;

            box.GetComponent<Box>().isInvincible = true;
        }
    }
}
