using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementsChara : MonoBehaviour
{
    public static MovementsChara Instance;
    private Controls controls;

    [Header("Movement Speeds")] 
    public float speedX;
    public float speedY;

    [Header("Deceleration")] 
    public float dragDeceleration;
    public float dragMultiplier;


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


    private void Start()
    {
        ManagerChara.Instance.rb.drag = dragDeceleration * dragMultiplier;
    }

    public void MoveCharacter()
    {
        Vector2 direction = controls.Character.Movements.ReadValue<Vector2>();

        ManagerChara.Instance.rb.AddForce(new Vector2(direction.x * speedX, direction.y* speedY), ForceMode2D.Force);
    }
}
