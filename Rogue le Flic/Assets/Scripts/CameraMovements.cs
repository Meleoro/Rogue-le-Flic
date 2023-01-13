using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using DG.Tweening;

public class CameraMovements : MonoBehaviour
{
    public static CameraMovements Instance;
    
    [HideInInspector] public Camera _camera;
    private Controls controls;

    private float newX;
    private float newY;

    public float multiplierMouse;
    public GameObject fondNoir;

    [HideInInspector] public bool canShake;
    [HideInInspector] public bool canMove;

    [Header("End Room Behavior")]
    [HideInInspector] public bool endRoom;
    [HideInInspector] public float timeZoom;
    [HideInInspector] public Vector2 ennemyPos;
    private float originalSize;
    private Vector2 originalPos;
    [HideInInspector] public float timerZoom;
    private float timerDezoom;


    [Header("Boss Room Behavior")]
    [HideInInspector] public bool bossEndRoom;
    [HideInInspector] public Vector2 posCamera;
    [HideInInspector] public bool stopDezoom;
    private bool doOnce;
    
    [Header("PlayerDeath")]
    private bool doOnce2;
    [HideInInspector] public bool playerDeath;


    private void Awake()
    {
        controls = new Controls();

        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);

        canShake = true;
        canMove = true;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }


    void Update()
    {
        if (playerDeath)
        {
            DeathBeahvior();
        }
        
        else
        {
            if (!endRoom && canMove && !bossEndRoom)
            {
                transform.position = NormalBehavior();

                originalSize = _camera.orthographicSize;
                originalPos = _camera.transform.position;
            }

            else if (canMove && !bossEndRoom)
                EndRoomBehavior();

            else if (canMove)
                BossEndRoomBehavior();
        }
    }


    Vector2 NormalBehavior()
    {
        Vector3 charaPos = ManagerChara.Instance.transform.position;
        GameObject activeRoom = MapManager.Instance.activeRoom;
        
        CameraManager limites = activeRoom.GetComponent<CameraManager>();
        
        
        float height = _camera.orthographicSize;
        float width = height * _camera.aspect;
        
        
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


        Vector2 mousePos = ReferenceCamera.Instance._camera.ScreenToViewportPoint(controls.Character.MousePosition.ReadValue<Vector2>());
        
        Vector2 newPos = new Vector2( mousePos.x * multiplierMouse - multiplierMouse / 2,  mousePos.y * multiplierMouse- multiplierMouse / 2);

        return new Vector3(newX + newPos.x, newY + newPos.y, transform.position.z);
    }


    void EndRoomBehavior()
    {
        //ZOOM
        if (timerDezoom <= 0)
        {
            timerZoom -= Time.deltaTime;
        
            _camera.orthographicSize = Mathf.Lerp(originalSize, originalSize / 1.2f, (1 - timerZoom / timeZoom) * 1.5f);

            transform.position = new Vector3(Mathf.Lerp(originalPos.x, ennemyPos.x,  1 - timerZoom / timeZoom), 
                Mathf.Lerp(originalPos.y, ennemyPos.y, 1 - timerZoom / timeZoom), transform.position.z);
            
            //SLOW MO
            Time.timeScale = Mathf.Lerp(1f, 0.4f, 1 - timerZoom / timeZoom);
            Time.fixedDeltaTime = 0.02f * Time.deltaTime;

            if (timerZoom <= -0.3f)
            {
                timerDezoom = 0.15f;
            }
        }
        
        // DEZOOM
        else
        {
            timerDezoom -= Time.deltaTime;
            
            _camera.orthographicSize = Mathf.Lerp(originalSize, originalSize / 1.2f, timerDezoom / 0.15f);

            Vector2 wantedPos = NormalBehavior();
            
            transform.position = new Vector3(Mathf.Lerp(wantedPos.x, ennemyPos.x, timerDezoom / 0.15f), 
                Mathf.Lerp(wantedPos.y, ennemyPos.y, timerDezoom / 0.15f), transform.position.z);
            
            Time.timeScale = Mathf.Lerp(1f, 0.4f, timerDezoom / 0.15f);
            Time.fixedDeltaTime = 0.02f * Time.deltaTime;

            if (timerDezoom <= 0)
            {
                endRoom = false;
            }
        }
    }

    void BossEndRoomBehavior()
    {
        if(!doOnce)
        {
            doOnce = true;

            _camera.DOOrthoSize(originalSize / 1.2f, timeZoom);

            transform.DOMove(posCamera, timeZoom - 0.2f);
        }
    }

    void DeathBeahvior()
    {
        if(!doOnce)
        {
            doOnce2 = true;

            _camera.DOOrthoSize(originalSize / 1.5f, timeZoom);

            transform.DOMove(posCamera, timeZoom - 0.2f);
        }
    }
}
