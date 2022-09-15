using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementsChara : MonoBehaviour
{
    public static MovementsChara Instance;
    private Controls controls;

    [Header("Movements X")] 
    public float speedX;
    
    [Header("Movements Y")] 
    public float speedY;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);

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


    public void MoveCharacter()
    {
        Vector2 direction = controls.Character.Movements.ReadValue<Vector2>();
        
        ManagerChara.Instance.rb.velocity = new Vector2(direction.x * speedX, direction.y* speedY);
    }
}
