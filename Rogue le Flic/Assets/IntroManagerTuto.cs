using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class IntroManagerTuto : MonoBehaviour
{
    private Vector3 posCharaFInal;
    private float finalZoom;

    public Image fond;

    private bool isInIntro;


    void Start()
    {
        isInIntro = true;

        posCharaFInal = ManagerChara.Instance.transform.position;
        finalZoom = ReferenceCamera.Instance._camera.orthographicSize;

        CameraMovements.Instance.transform.position = posCharaFInal;

        ReferenceCamera.Instance.finalCinematicChara = true;

        fond.gameObject.SetActive(true);

        ManagerChara.Instance.noControl = true;
        CameraMovements.Instance.canMove = false;

        StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {

        CameraMovements.Instance.canMove = true;
        ReferenceCamera.Instance._camera.DOOrthoSize(finalZoom / 2, 0);

        ManagerChara.Instance.transform.position = posCharaFInal + new Vector3(20, 0, 0);
        ManagerChara.Instance.anim.SetBool("isWalking", true);

        ManagerChara.Instance.transform.DOMove(posCharaFInal, 3).SetEase(Ease.Linear);


        yield return new WaitForSeconds(3);


        fond.DOFade(0, 1);

        ReferenceCamera.Instance._camera.DOOrthoSize(finalZoom, 2);

        isInIntro = false;
        CameraMovements.Instance.canMove = true;

        CameraMovements.Instance.isInTransition = true;
        CameraMovements.Instance.departTransition = CameraMovements.Instance.transform.position;
        CameraMovements.Instance.timerTransition = 1;

        ManagerChara.Instance.noControl = false;

        yield return new WaitForSeconds(1);

        ReferenceCamera.Instance.finalCinematicChara = false;
    }

    private void Update()
    {
        if (isInIntro)
        {
            CameraMovements.Instance.canMove = false;
        }
    }
}
