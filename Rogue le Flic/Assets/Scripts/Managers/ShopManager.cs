using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ShopManager : MonoBehaviour
{

    public int[,] shopItems = new int[5,5];
    public float coins;
    public TMP_Text CoinsTXT;
    
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        CoinsTXT.text = "Coins:" + CounterToken.instance.currentTokenCount.ToString();

        
        //ID's
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;
        
        
        //Price
        shopItems[2, 1] = 1;
        shopItems[2, 2] = 2;
        shopItems[2, 3] = 3;
        shopItems[2, 4] = 4;
        
        
        //Quantity
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0; 
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;
    }

    void Update()
    {
        CoinsTXT.text = "Coins:" + CounterToken.instance.currentTokenCount.ToString();
    }
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>()
            .currentSelectedGameObject;

        if (CounterToken.instance.currentTokenCount >= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID])
        {
            CounterToken.instance.currentTokenCount -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
            shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID] ++;
            CoinsTXT.text = "Coins:" + CounterToken.instance.currentTokenCount.ToString();
            ButtonRef.GetComponent<ButtonInfo>().QuantityTxt.text =
                shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID].ToString();

           // newSprite = ButtonRef.GetComponent<Sprite>();
           //spriteRenderer.sprite = newSprite;
            
            ChangeSprite();

        }
    }

    public void ChangeSprite()
    {
        spriteRenderer.sprite = newSprite;
    }

}
