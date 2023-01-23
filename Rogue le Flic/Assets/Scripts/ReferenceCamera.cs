using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReferenceCamera : MonoBehaviour
{
    public static ReferenceCamera Instance;

    public Camera _camera;
    public Image fondNoir;

    public bool finalCinematic;
    public bool finalCinematicChara;
    
    [Header("Splash")]
    public GameObject splash;
    public SpriteRenderer bossSprite;
    public SpriteRenderer bossName;

    [Header("Différents splash")] 
    public Sprite beaver;
    public Sprite frog;
    public Sprite turtle;

    [Header("Differents noms")] 
    public Sprite beaverName;
    public Sprite frogName;
    public Sprite turtleName;

    public Sprite beaverTurtle;
    public Sprite turtleFrog;
    public Sprite frogBeaver;

    public Sprite all;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);

        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        CameraMovements.Instance._camera = _camera;

        ReferenceUI.Instance.canvas.worldCamera = _camera;
    }
}
