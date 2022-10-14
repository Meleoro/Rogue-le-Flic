using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using DG.Tweening;

public class CameraMovements : MonoBehaviour
{
    private Camera camera;
    private Controls controls;

    private float newX;
    private float newY;

    public float multiplierMouse;
    

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }


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
            newY =  charaPos.y;
        }

        else
        {
            if (limites.limitUp.transform.position.y <= charaPos.y + height)
            {
                newY = limites.limitUp.transform.position.y - height;
            }
            else
            {
                newY = limites.limitBottom.transform.position.y + height;
            }
        }
        

        if (limites.limitLeft.transform.position.x < charaPos.x - width && 
            limites.limitRight.transform.position.x > charaPos.x + width)
        {
            newX = charaPos.x;
        }
        
        else
        {
            if (limites.limitLeft.transform.position.x >= charaPos.x - width)
            {
                newX = limites.limitLeft.transform.position.x + width;
            }
            else
            {
                newX = limites.limitRight.transform.position.x - width;
            }
        }


        Vector2 mousePos = ReferenceCamera.Instance.camera.ScreenToViewportPoint(controls.Character.MousePosition.ReadValue<Vector2>());
        
        Vector2 newPos = new Vector2( mousePos.x * multiplierMouse - multiplierMouse / 2,  mousePos.y * multiplierMouse- multiplierMouse / 2);

        transform.position = new Vector3(newX + newPos.x, newY + newPos.y, transform.position.z);
        
    }
}
