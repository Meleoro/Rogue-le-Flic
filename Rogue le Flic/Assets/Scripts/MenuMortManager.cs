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

    void Start()
    {
        
    }


    public void Restart()
    {
        SceneManager.LoadScene("Level 1");
        
        Destroy(DontDestroyOnLoadScript2.Instance.gameObject);
    }
}
