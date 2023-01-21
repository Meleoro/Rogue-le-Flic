using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeColliderBossFrog : MonoBehaviour
{
    [SerializeField] private FrogBoss frog;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask playerLayer;

    private RaycastHit2D hit;
    private RaycastHit2D hit2;
    private bool touchedSomething;
    private bool touchedPlayer;

    private void Start()
    {
        frog = GetComponentInParent<FrogBoss>();
    }


    private void Update()
    {
        frog.canShoot = true;
        
        Debug.DrawRay(transform.position, (ManagerChara.Instance.transform.position - transform.position), Color.red, 0.2f);

        hit = Physics2D.Raycast(transform.position, ManagerChara.Instance.transform.position - transform.position, 10, obstacleLayer);
        touchedSomething = Physics2D.Raycast(transform.position, (ManagerChara.Instance.transform.position - transform.position), 10, obstacleLayer);

        if (touchedSomething)
        {
            if (hit.collider.CompareTag("Wall"))
            {
                frog.canShoot = false;
            }
        }

        if (Mathf.Abs((ManagerChara.Instance.transform.position - transform.position).x) > 16 ||
            Mathf.Abs((ManagerChara.Instance.transform.position - transform.position).y) > 9)
        {
            frog.canShoot = false;
        }
    }
}
