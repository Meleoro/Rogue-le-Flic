using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeColliderFrog : MonoBehaviour
{
    [SerializeField] private Frog frog;
    [SerializeField] private LayerMask obstacleLayer;


    private RaycastHit raycastHit;


    private void Update()
    {
        frog.canShoot = true;

        Debug.DrawRay(transform.position, ManagerChara.Instance.transform.position - transform.position, Color.red, 0.2f);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, ManagerChara.Instance.transform.position - transform.position, frog.distanceShotTrigger, obstacleLayer);

        Debug.Log(hit.collider.tag);
        if (hit.collider.CompareTag("Wall"))
        {
            Debug.Log(12);
            frog.canShoot = false;
        }
    }
}
