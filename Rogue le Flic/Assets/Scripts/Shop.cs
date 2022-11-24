using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] private RectTransform shopkeeperUI;
    [SerializeField] private RectTransform detailsUI;

    [Header("Ouverture / Fermeture Shop")] 
    [SerializeField] private Vector2 posShopkeeper;
    [SerializeField] private Vector2 posDetails;
    [SerializeField] private float openingDuration;
    [SerializeField] private AnimationCurve rotationItems;
    private float timerEnter;
    private Vector2 originalPosShopkeeper;
    private Vector2 originalPosDetails;
    [SerializeField] private float closingDuration;

    [Header("Marchandise")] 
    [SerializeField] private List<GameObject> items;
    [SerializeField] private List<GameObject> weapons;
    private List<GameObject> currentItems = new List<GameObject>();

    [Header("Details")] 
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [Header("Références")]
    [SerializeField] private Image item1;
    [SerializeField] private Image item2;
    [SerializeField] private Image item3;
    [SerializeField] private RectTransform ancrage1;
    [SerializeField] private RectTransform ancrage2;
    [SerializeField] private RectTransform ancrage3;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private Image fond;

    [Header("Others")] 
    private bool isOpen;
    private bool canUseShop;


    private void Start()
    {
        originalPosShopkeeper = shopkeeperUI.localPosition;
        originalPosDetails = detailsUI.localPosition;
        
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

        if (timerEnter > 0)
        {
            timerEnter -= Time.deltaTime;
            
            ancrage1.rotation = Quaternion.Euler(0, 0, rotationItems.Evaluate(4f - timerEnter) * 100);
            ancrage2.rotation = Quaternion.Euler(0, 0, rotationItems.Evaluate(4f - timerEnter) * 100);
            ancrage3.rotation = Quaternion.Euler(0, 0, rotationItems.Evaluate(4f - timerEnter) * 100);
        }
    }


    void OpenShop()
    {
        shopkeeperUI.DOLocalMove(posShopkeeper, openingDuration);
        detailsUI.DOLocalMove(posDetails, openingDuration);
        fond.DOFade(0.8f, 0.5f);
        
        ManagerChara.Instance.noControl = true;

        isOpen = true;

        timerEnter = 4f;
    }

    
    void CloseShop()
    {
        shopkeeperUI.DOLocalMove(originalPosShopkeeper, closingDuration);
        detailsUI.DOLocalMove(originalPosDetails, closingDuration);
        fond.DOFade(0f, 0.5f);
        
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
                currentItems.Add(weapons[choice]);
            }
            
            else if (k == 1)
            {
                int choice = Random.Range(0, items.Count);
                
                item2.sprite = items[choice].GetComponent<SpriteRenderer>().sprite;
                currentItems.Add(items[choice]);
            }

            else
            {
                int choice = Random.Range(0, items.Count);
                
                item3.sprite = items[choice].GetComponent<SpriteRenderer>().sprite;
                currentItems.Add(items[choice]);
            }
        }
    }


    public void PurshaseItem(int itemID)
    {
        if (itemID == 1)
        {
            GameObject newGun = Instantiate(currentItems[0]);
            
            newGun.GetComponent<Gun>().canBePicked = true;
            newGun.GetComponent<Gun>().PickWeapon();
            
            item1.enabled = false;
        }

        else
        {
            GameObject newModule = Instantiate(currentItems[itemID - 1]);
            
            newModule.GetComponent<Module>().OpenChoice();

            if (itemID == 2)
                item2.enabled = false;
            
            else
                item3.enabled = false;
        }
    }


    public void ButtonSelected(int buttonNumber)
    {
        if (buttonNumber == 1)
        {
            itemName.text = currentItems[buttonNumber - 1].GetComponent<Gun>().itemName;
            itemDescription.text = currentItems[buttonNumber - 1].GetComponent<Gun>().itemDescription;
        }

        else
        {
            itemName.text = currentItems[buttonNumber - 1].GetComponent<Module>().itemName;
            itemDescription.text = currentItems[buttonNumber - 1].GetComponent<Module>().itemDescription;
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
