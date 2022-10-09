using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementsChara : MonoBehaviour
{
    public static MovementsChara Instance;
    public Controls controls;

    [Header("Movement Speeds")] 
    public float speedX;
    public float speedY;

    [Header("Deceleration")] 
    public float dragDeceleration;
    public float dragMultiplier;


    private void Awake()
    {
        controls = new Controls();

        Instance = this;
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

    public void RotateCharacter()
    {
        if(ManagerChara.Instance.controls.Character.Movements.ReadValue<Vector2>().x > 0.1f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        else if (ManagerChara.Instance.controls.Character.Movements.ReadValue<Vector2>().x < -0.1f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}
