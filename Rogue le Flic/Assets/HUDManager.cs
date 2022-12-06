using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;
    
    public TextMeshProUGUI ammo;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        ammo.text = currentAmmo + "/" + maxAmmo;
    }
}
