using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shop : MonoBehaviour
{
    [SerializeField] private RectTransform shopkeeperUI;

    [Header("Ouverture Shop")] 
    [SerializeField] private Vector2 posShopkeeper;
    [SerializeField] private float openingDuration;

    [Header("Fermeture Shop")]
    private Vector2 originalPosShopkeeper;
    [SerializeField] private float closingDuration;

    [Header("Others")] 
    private bool isOpen;
    private bool canUseShop;


    private void Start()
    {
        originalPosShopkeeper = shopkeeperUI.localPosition;
    }

    
    private void Update()
    {
        if (ManagerChara.Instance.controls.Character.Enter.WasPressedThisFrame() && canUseShop)
        {
            if(!isOpen)
                OpenShop();
            
            else
                CloseShop();
        }
    }


    void OpenShop()
    {
        shopkeeperUI.DOLocalMove(posShopkeeper, openingDuration);
        ManagerChara.Instance.noControl = true;

        isOpen = true;
    }

    void CloseShop()
    {
        shopkeeperUI.DOLocalMove(originalPosShopkeeper, closingDuration);
        ManagerChara.Instance.noControl = false;

        isOpen = false;
    }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            canUseShop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canUseShop = false;
        }
    }
}
