using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Viseur : MonoBehaviour
{
    public static Viseur Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        transform.position = ReferenceCamera.Instance._camera.ScreenToWorldPoint(ManagerChara.Instance.controls.Character.MousePosition.ReadValue<Vector2>()) + new Vector3(0, 0, 10);
    }

    public void GrossissementShoot(float duree)
    {
        transform.DOScale(new Vector3(0.7f, 0.7f, 1), duree/3).OnComplete((() => transform.DOScale(new Vector3(0.5f, 0.5f, 1), (duree/3) * 2))); 
    }
}
