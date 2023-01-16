using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class DashChara : MonoBehaviour
{
    public static DashChara Instance;
    
    public float dashForceX;
    public float dashForceY;

    public float dureeAjoutForce;
    private float timerForce;
    private Vector2 direction;

    public float noHitTime;

    public LayerMask normalLayer;
    public LayerMask invincibleLayer;

    [Header("Effets")] 
    [SerializeField] private Volume dashEffects;
    [SerializeField] private Volume dashBlur;
    private float timerEffects;

    [Header("Camera Shake")] 
    public float duration;
    public float amplitude;

    private bool doOnce;


    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    private void Update()
    {
        if (timerForce > 0)
        {
            timerForce -= Time.deltaTime;
        }

        if (timerEffects > 0)
        {
            timerEffects -= Time.deltaTime;
            dashBlur.weight = 1;
        }
        else
        {
            dashBlur.weight = 0;
        }
    }


    private void FixedUpdate()
    {
        if (timerForce > 0)
        {
            ManagerChara.Instance.rb.AddForce(new Vector2(direction.x * dashForceX, direction.y * dashForceY), ForceMode2D.Force);
        }
        
        if (timerEffects > 0)
        {
            dashEffects.weight = timerEffects / noHitTime;
        }
    }


    public void Dash()
    {
        timerForce = dureeAjoutForce;
        timerEffects = noHitTime;
        
        ReferenceCamera.Instance.transform.DOShakePosition(duration, amplitude);
        
        direction = ManagerChara.Instance.controls.Character.Movements.ReadValue<Vector2>();

        ManagerChara.Instance.rb.AddForce(new Vector2(direction.x * dashForceX, direction.y * dashForceY), ForceMode2D.Force);
    }
}
