using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuMortManager : MonoBehaviour
{
    public static MenuMortManager Instance;

    public Image fondMort;
    public TextMeshProUGUI scoretext;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }


    public void Restart()
    {
        StartCoroutine(FonduManager.Instance.ChangeScene("Level 1", true));
    }
}
