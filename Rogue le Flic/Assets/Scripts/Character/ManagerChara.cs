using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerChara : MonoBehaviour
{
    public static ManagerChara Instance;
    
    [HideInInspector] public Rigidbody2D rb;
    public GameObject activeGun;
    public GameObject stockWeapon;

    public Controls controls;
    public bool noControl;

    [Header("Dash")] 
    private bool isDashing;
    private float timerDash;
    
    [Header("Kick")] 
    private bool isKicking;
    private float timerKick;

    [Header("Autres")] 
    public float switchCooldown;
    public bool canSwitch;
    
    

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

        canSwitch = true;
    }

    private void Update()
    {
        if (!noControl)
        {
            MovementsChara.Instance.RotateCharacter();

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
                StartCoroutine(KickChara.Instance.Kick());
            }

            else if (isDashing)
            {
                timerDash -= Time.deltaTime;

                if (timerDash <= 0)
                {
                    isDashing = false;
                }
            }
            
            // SWITCH WEAPONS
            if (controls.Character.SwitchWeapon.ReadValue<float>() != 0 && canSwitch)
            {
                SwitchWeapons();
            }
        }
    }

    void SwitchWeapons()
    {
        if (stockWeapon != null)
        {
            GameObject save = activeGun;

            activeGun = stockWeapon;
            stockWeapon = save;

            activeGun.GetComponent<Gun>().isStocked = false;
            stockWeapon.GetComponent<Gun>().isStocked = true;
            
            stockWeapon.GetComponent<Gun>().Stocking();

            StartCoroutine(CooldownSwitch());
        }
    }

    private IEnumerator CooldownSwitch()
    {
        canSwitch = false;
        
        yield return new WaitForSeconds(switchCooldown);

        canSwitch = true;
    }
}
