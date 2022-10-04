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
    private bool noControl;

    [Header("Dash")] 
    private bool isDashing;
    private float timerDash;
    
    [Header("Kick")] 
    private bool isKicking;
    private float timerKick;
    
    

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
        if (!noControl)
        {
            if(!isDashing)
                MovementsChara.Instance.MoveCharacter();

            if (controls.Character.Dash.WasPerformedThisFrame() && !isDashing)
            {
                DashChara.Instance.Dash();

                timerDash = DashChara.Instance.noHitTime;
                isDashing = true;
            }
            
            if (controls.Character.Kick.WasPerformedThisFrame() && !isKicking)
            {
                KickChara.Instance.Kick();
            }

            else if (isDashing)
            {
                timerDash -= Time.deltaTime;

                if (timerDash <= 0)
                {
                    isDashing = false;
                }
            }
        }
    }
}
