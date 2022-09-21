using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovements : MonoBehaviour
{
    private Camera camera;

    
    private void Start()
    {
        camera = ReferenceCamera.Instance.camera;
    }

    void Update()
    {
        Vector3 charaPos = ManagerChara.Instance.transform.position;
        GameObject activeRoom = MapManager.Instance.activeRoom;
        CameraManager limites = activeRoom.GetComponent<CameraManager>();

        float height = camera.orthographicSize;
        float width = height * camera.aspect;
        

        if (limites.limitUp.transform.position.y > charaPos.y + height && 
            limites.limitBottom.transform.position.y < charaPos.y - height)
        {
            transform.position = new Vector3(transform.position.x, charaPos.y, transform.position.z);
        }

        if (limites.limitLeft.transform.position.x < charaPos.x - width && 
            limites.limitRight.transform.position.x > charaPos.x + width)
        {
            transform.position = new Vector3(charaPos.x, transform.position.y, transform.position.z);
        }
    }
}
