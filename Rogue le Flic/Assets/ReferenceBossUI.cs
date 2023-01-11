using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReferenceBossUI : MonoBehaviour
{
    public static ReferenceBossUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject object1;
    public GameObject object2;
    public GameObject object3;

    public Image healthBar1;
    public Image healthBar2;
    public Image healthBar3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
