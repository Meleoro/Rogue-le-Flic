using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerChara : MonoBehaviour
{
    public static ManagerChara Instance;
    
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public GameObject activeGun;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);

        rb = GetComponent<Rigidbody2D>();
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
