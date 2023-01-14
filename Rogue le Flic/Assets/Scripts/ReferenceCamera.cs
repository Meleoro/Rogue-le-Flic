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
    public TextMeshProUGUI bossName;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _camera = GetComponent<Camera>();

        CameraMovements.Instance._camera = _camera;
    }
}
