using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public int moneyDropPerEnnemy;

    public int modulePrice;
    public int healthPrice;
    
    [Header("Boss")]
    public int moneyBossSpare;
    public List<GameObject> itemsKick;
    public int nbrCoeurs;

    [Header("Reference")]
    public GameObject coin;
    public GameObject health;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);
    }

    
}
