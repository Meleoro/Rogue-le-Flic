using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeColliderFrog : MonoBehaviour
{
    [SerializeField] private Frog frog;
    [SerializeField] private LayerMask obstacleLayer;


    private RaycastHit2D hit;
    private bool touchedSomething;


    private void Update()
    {
        frog.canShoot = true;

        Debug.DrawRay(transform.position, ManagerChara.Instance.transform.position - transform.position, Color.red, 0.2f);

        hit = Physics2D.Raycast(transform.position, ManagerChara.Instance.transform.position - transform.position, frog.distanceShotTrigger, obstacleLayer);
        touchedSomething = Physics2D.Raycast(transform.position, ManagerChara.Instance.transform.position - transform.position, frog.distanceShotTrigger, obstacleLayer);

        if (touchedSomething)
        {
            if (hit.collider.CompareTag("Wall"))
            {
                frog.canShoot = false;
            }
        }
    }
}
