using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TriggerTuto : MonoBehaviour
{
    public Ennemy beaverToSpawn;

    public Vector3 posCamera;

    private bool doOnce;


    private void Start()
    {
        doOnce = false;
    }


    IEnumerator SpawnCinematique()
    {
        CameraMovements.Instance.canMove = false;
        ManagerChara.Instance.noControl = true;

        CameraMovements.Instance.transform.DOMove(posCamera, 1.5f);

        yield return new WaitForSeconds(1.5f);

        beaverToSpawn.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        CameraMovements.Instance.departTransition = CameraMovements.Instance.transform.position;
        CameraMovements.Instance.timerTransition = 1.5f;
        CameraMovements.Instance.isInTransition = true;

        CameraMovements.Instance.canMove = true;
        ManagerChara.Instance.noControl = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !doOnce)
        {
            doOnce = true;
            StartCoroutine(SpawnCinematique());
        }
    }
}

