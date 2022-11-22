using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Shop : MonoBehaviour
{
    [SerializeField] private RectTransform shopkeeperUI;

    [Header("Ouverture Shop")] 
    [SerializeField] private Vector2 posShopkeeper;
    [SerializeField] private float openingDuration;

    [Header("Fermeture Shop")]
    private Vector2 originalPosShopkeeper;
    [SerializeField] private float closingDuration;

    [Header("Marchandise")] 
    [SerializeField] private List<GameObject> items;
    [SerializeField] private List<GameObject> weapons;
    [SerializeField] private Image item1;
    [SerializeField] private Image item2;
    [SerializeField] private Image item3;

    [Header("Others")] 
    private bool isOpen;
    private bool canUseShop;


    private void Start()
    {
        originalPosShopkeeper = shopkeeperUI.localPosition;
        
        ChoiceItems();
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

    
    void ChoiceItems()
    {
        for (int k = 0; k < 3; k++)
        {
            if (k == 0)
            {
                int choice = Random.Range(0, weapons.Count);
                
                item1.sprite = weapons[choice].GetComponent<SpriteRenderer>().sprite;
            }
            
            else if (k == 1)
            {
                int choice = Random.Range(0, items.Count);
                
                item2.sprite = items[choice].GetComponent<SpriteRenderer>().sprite;
            }

            else
            {
                int choice = Random.Range(0, items.Count);
                
                item3.sprite = items[choice].GetComponent<SpriteRenderer>().sprite;
            }
        }
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
