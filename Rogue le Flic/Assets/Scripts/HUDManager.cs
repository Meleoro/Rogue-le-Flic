using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [SerializeField] private GameObject ammoUI;
    public Image gunImage;
    public RectTransform gunImagePos;
    public TextMeshProUGUI ammo;

    private Vector2 normalPos;
    [SerializeField] private Vector2 bowPos;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
            Destroy(gameObject);
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
    }

    public void ReplaceImage(int maxAmmo)
    {
        if (maxAmmo == 1)
        {
            gunImage.rectTransform.localPosition = bowPos;
        }

        else
        {
            gunImage.rectTransform.localPosition = normalPos;
        }
        
        gunImagePos.DOLocalMoveY(gunImagePos.localPosition.y + 8, 0.1f).OnComplete((() =>
            gunImagePos.DOLocalMoveY(gunImagePos.localPosition.y - 8, 0.1f)));
    }
}
