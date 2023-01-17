using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceUI : MonoBehaviour
{
    public static ReferenceUI Instance;

    public Canvas canvas;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);
    }
}
