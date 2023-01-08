using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceChoice : MonoBehaviour
{
    public static ReferenceChoice Instance;

    public RectTransform spare;
    public RectTransform kick;


    private void Awake()
    {
        Instance = this;
    }
}
