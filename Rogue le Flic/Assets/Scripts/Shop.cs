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
    [SerializeField] private RectTransform moneyUI;

    [Header("Ouverture / Fermeture Shop")] 
    [SerializeField] private Vector2 posShopkeeper;
    [SerializeField] private Vector2 posDetails;
    [SerializeField] private Vector2 posMoney;
    [SerializeField] private float openingDuration;
    [SerializeField] private AnimationCurve rotationItems;
    private float timerEnter;
    private Vector2 originalPosShopkeeper;
    private Vector2 originalPosDetails;
    private Vector2 originalPosMoney;
    [SerializeField] private float closingDuration;

    [Header("Marchandise")] 
    [SerializeField] private List<GameObject> items;
    [SerializeField] private List<GameObject> weapons;
    private List<GameObject> currentItems = new List<GameObject>();

    [Header("Details")] 
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI itemPrice;
    
    [Header("Current Money")]
    [SerializeField] private TextMeshProUGUI currentMoney;

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
        originalPosDetails = moneyUI.localPosition;
        
        ChoiceItems();
    }

    
    private void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
        
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

        currentMoney.text = CoinManager.Instance.currentCoins + "";
    }


    void OpenShop()
    {
        MenuPauseManager.Instance.otherMenuActive = true;
        
        shopkeeperUI.DOLocalMove(posShopkeeper, openingDuration);
        detailsUI.DOLocalMove(posDetails, openingDuration);
        moneyUI.DOLocalMove(posMoney, openingDuration);
        fond.DOFade(0.8f, 0.5f);
        
        ManagerChara.Instance.noControl = true;

        isOpen = true;
        CameraMovements.Instance.canMove = false;

        timerEnter = 4f;
    }

    
    void CloseShop()
    {
        shopkeeperUI.DOLocalMove(originalPosShopkeeper, closingDuration);
        detailsUI.DOLocalMove(originalPosDetails, closingDuration);
        moneyUI.DOLocalMove(originalPosMoney, closingDuration);
        fond.DOFade(0f, 0.5f);
        
        ManagerChara.Instance.noControl = false;
        CameraMovements.Instance.canMove = true;

        isOpen = false;
        
        CameraMovements.Instance.timerTransition = 1;
        CameraMovements.Instance.isInTransition = true;
        CameraMovements.Instance.departTransition = CameraMovements.Instance.transform.position;

        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
        
        MenuPauseManager.Instance.otherMenuActive = false;
    }


    void ChoiceItems()
    {
        for (int k = 0; k < 3; k++)
        {
            if (k == 0)
            {
                int choice = Random.Range(0, weapons.Count);
                
                item1.sprite = weapons[choice].GetComponent<SpriteRenderer>().sprite;
                item1.SetNativeSize();
                currentItems.Add(weapons[choice]);
            }
            
            else if (k == 1)
            {
                int choice = Random.Range(0, items.Count);
                
                item2.sprite = items[choice].GetComponent<SpriteRenderer>().sprite;
                item2.SetNativeSize();
                currentItems.Add(items[choice]);

                items.RemoveAt(choice);
            }

            else
            {
                /*int choice = Random.Range(0, items.Count);
                
                item3.sprite = items[choice].GetComponent<SpriteRenderer>().sprite;
                item3.SetNativeSize();
                currentItems.Add(items[choice]);*/

                item3.sprite = HealthManager.Instance.vie;
                item3.SetNativeSize();
            }
        }
    }


    public void PurshaseItem(int itemID)
    {
        Debug.Log(CoinManager.Instance.currentCoins);

        if (itemID == 1)
        {
            if (CoinManager.Instance.currentCoins >= currentItems[itemID - 1].GetComponent<Gun>().gunData.itemPrice)
            {
                GameObject newGun = Instantiate(currentItems[0]);

                CoinManager.Instance.currentCoins -= currentItems[itemID - 1].GetComponent<Gun>().gunData.itemPrice;

                newGun.GetComponent<Gun>().canBePicked = true;
                newGun.GetComponent<Gun>().PickWeapon();
            
                item1.enabled = false;
            }
        }

        else if(itemID == 2 && CoinManager.Instance.currentCoins >= MoneyManager.Instance.modulePrice)
        {
            GameObject newModule = Instantiate(currentItems[itemID - 1]);

            CoinManager.Instance.currentCoins -= MoneyManager.Instance.modulePrice;

            newModule.GetComponent<Module>().OpenChoice();
            
            item2.enabled = false;
        }
        
        else if (itemID == 3 && CoinManager.Instance.currentCoins >= MoneyManager.Instance.healthPrice)
        {
            HealthManager.Instance.AddHealth();
            
            CoinManager.Instance.currentCoins -= MoneyManager.Instance.healthPrice;
            
            item3.enabled = false;
        }
    }


    public void ButtonSelected(int buttonNumber)
    {
        if (buttonNumber == 1)
        {
            itemName.text = currentItems[buttonNumber - 1].GetComponent<Gun>().gunData.itemName;
            itemDescription.text = currentItems[buttonNumber - 1].GetComponent<Gun>().gunData.itemDesciption;
            itemPrice.text = currentItems[buttonNumber - 1].GetComponent<Gun>().gunData.itemPrice + "$";
        }

        else if (buttonNumber == 2)
        {
            itemName.text = currentItems[buttonNumber - 1].GetComponent<Module>().itemName;
            itemDescription.text = currentItems[buttonNumber - 1].GetComponent<Module>().itemDescription;
            itemPrice.text = MoneyManager.Instance.modulePrice + "$";
        }

        else
        {
            itemName.text = "Soin";
            itemDescription.text = "Rend un point de vie";
            itemPrice.text = MoneyManager.Instance.healthPrice + "$";
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
