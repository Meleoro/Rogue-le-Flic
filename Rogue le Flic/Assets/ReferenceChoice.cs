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

    [Header("HurtBosses")] 
    public GameObject beaverHurt;
    public GameObject frogBoss;
    public GameObject turtleBoss;


    private void Awake()
    {
        Instance = this;
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
