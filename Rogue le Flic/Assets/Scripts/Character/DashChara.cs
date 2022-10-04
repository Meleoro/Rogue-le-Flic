using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DashChara : MonoBehaviour
{
    public static DashChara Instance;
    
    public float dashForceX;
    public float dashForceY;

    public float noHitTime;

    [Header("Camera Shake")] 
    public float duration;
    public float amplitude;


    private void Awake()
    {
        Instance = this;
    }


    public void Dash()
    {
        ReferenceCamera.Instance.transform.DOShakePosition(duration, amplitude);
        
        Vector2 direction = ManagerChara.Instance.controls.Character.Movements.ReadValue<Vector2>();

        ManagerChara.Instance.rb.AddForce(new Vector2(direction.x * dashForceX, direction.y * dashForceY), ForceMode2D.Impulse);
    }
}
