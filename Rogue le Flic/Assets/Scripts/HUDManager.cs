using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [SerializeField] private GameObject ammoUI;
    [SerializeField] private Image gunImage;
    public TextMeshProUGUI ammo;

    private Vector2 normalPos;
    [SerializeField] private Vector2 bowPos;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        normalPos = gunImage.rectTransform.localPosition;
        
        ammoUI.SetActive(false);
    }

    
    public void TakeWeapon()
    {
        ammoUI.SetActive(true);
    }

    public void UpdateAmmo(int currentAmmo, int maxAmmo, Sprite sprite)
    {
        ammo.text = currentAmmo + "/" + maxAmmo;

        gunImage.sprite = sprite;
        gunImage.SetNativeSize();
        
        if (maxAmmo == 1)
        {
            gunImage.rectTransform.localPosition = bowPos;
        }

        else
        {
            gunImage.rectTransform.localPosition = normalPos;
        }
    }
}
