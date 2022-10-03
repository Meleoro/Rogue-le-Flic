using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerChara : MonoBehaviour
{
    public static ManagerChara Instance;
    
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public GameObject activeGun;

    public Controls controls;
    
    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);

        rb = GetComponent<Rigidbody2D>();
        
        controls = new Controls();
    }
    
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MovementsChara.Instance.MoveCharacter();
    }
}
