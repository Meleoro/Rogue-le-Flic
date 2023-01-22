using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Heal : MonoBehaviour
{
    public Transform parent;
    
    private bool canInteract;
    private bool goUp;
    private bool goDown;

    private void Start()
    {
        goUp = true;
    }

    private void Update()
    {
        if (canInteract)
        {
            if (ManagerChara.Instance.controls.Character.Enter.WasPerformedThisFrame())
            {
                
            }
        }

        if (goUp)
        {
            goUp = false;
            transform.DOLocalMoveY(transform.localPosition.y + 0.15f, 0.8f).OnComplete((() => goDown = true)).SetEase(Ease.InOutQuad);
        }
        
        else if (goDown)
        {
            goDown = false;
            transform.DOLocalMoveY(transform.localPosition.y - 0.15f, 0.8f).OnComplete((() => goUp = true)).SetEase(Ease.InOutQuad);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            HealthManager.Instance.AddHealth();
            Destroy(gameObject);
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
}
