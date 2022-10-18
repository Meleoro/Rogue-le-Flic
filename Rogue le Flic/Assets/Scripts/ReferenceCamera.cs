using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceCamera : MonoBehaviour
{
    public static ReferenceCamera Instance;

    public Camera _camera;

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
    }
}
