using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ButtonInfo : MonoBehaviour
{


    public int ItemID;
    public TMP_Text PriceTxt;
    public TMP_Text QuantityTxt;
    public GameObject ShopManager;
    

    // Update is called once per frame
    void Update()
    {
        PriceTxt.text = "Price: $" + ShopManager.GetComponent<ShopManager>().shopItems[2, ItemID].ToString();
        QuantityTxt.text = ShopManager.GetComponent<ShopManager>().shopItems[2, ItemID].ToString();

    }
}
