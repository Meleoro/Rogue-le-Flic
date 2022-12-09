using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MovementsChara : MonoBehaviour
{
    public static MovementsChara Instance;
    public Controls controls;

    [HideInInspector] public Vector2 direction;
    [HideInInspector] public bool lookLeft;

    [Header("Movement Speeds")] 
    public float speedX;
    public float speedY;

    [Header("Deceleration")] 
    public float dragDeceleration;
    public float dragMultiplier;

    [Header("Références")] 
    public GameObject sprite;


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
        direction = controls.Character.Movements.ReadValue<Vector2>();

        ManagerChara.Instance.rb.AddForce(new Vector2(direction.x * speedX, direction.y* speedY) * 10, ForceMode2D.Force);

        if (Mathf.Abs(direction.x) > 0.1f || Mathf.Abs(direction.y) > 0.1f)
        {
            ManagerChara.Instance.anim.SetBool("isWalking", true);
        }
        else
        {
            ManagerChara.Instance.anim.SetBool("isWalking", false);
        }
    }

    public void RotateCharacter()
    {
        if (!ManagerChara.Instance.activeGun)
        {
            if(ManagerChara.Instance.controls.Character.Movements.ReadValue<Vector2>().x > 0.1f)
            {
                ManagerChara.Instance.anim.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);

                sprite.transform.localPosition = ManagerChara.Instance.posRight;
            }

            else if (ManagerChara.Instance.controls.Character.Movements.ReadValue<Vector2>().x < -0.1f)
            {
                ManagerChara.Instance.anim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

                sprite.transform.localPosition = ManagerChara.Instance.posLeft;  
            }
        }

        else
        {
            if (lookLeft)
            {
                ManagerChara.Instance.anim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

                sprite.transform.localPosition = ManagerChara.Instance.posLeft;  
            }

            else
            {
                ManagerChara.Instance.anim.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);

                sprite.transform.localPosition = ManagerChara.Instance.posRight;
            }
        }
    }
}
