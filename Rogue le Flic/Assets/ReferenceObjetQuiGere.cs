using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceObjetQuiGere : MonoBehaviour
{
    public static ReferenceObjetQuiGere Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);
    }
}
