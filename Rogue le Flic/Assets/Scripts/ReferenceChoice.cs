using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceChoice : MonoBehaviour
{
    public static ReferenceChoice Instance;

    public RectTransform spare;
    public RectTransform kick;

    public bool kicked;
    public bool spared;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);
    }

    public void Kicked()
    {
        kicked = true;
    }

    public void Spared()
    {
        spared = true;
    }
}
