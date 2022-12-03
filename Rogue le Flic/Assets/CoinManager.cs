using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [SerializeField] private TextMeshProUGUI coinTXT;

    private int currentCoins;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);
    }

    public void AddCoin(int coinNbr)
    {
        currentCoins += coinNbr;

        coinTXT.text = "" + currentCoins;
    }
}
