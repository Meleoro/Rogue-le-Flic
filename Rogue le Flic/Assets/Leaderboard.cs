using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public TextMeshProUGUI score1;
    public TextMeshProUGUI score2;
    public TextMeshProUGUI score3;
    public TextMeshProUGUI score4;
    public TextMeshProUGUI score5;
    
    
    private void Start()
    {
        InitializePlayerPref();
        
        VerifyScores();
    }


    public void VerifyScores()
    {
        score1.text = "1 - " + PlayerPrefs.GetFloat("bestScore1");
        score2.text = "2 - " + PlayerPrefs.GetFloat("bestScore2");
        score3.text = "3 - " + PlayerPrefs.GetFloat("bestScore3");
        score4.text = "4 - " + PlayerPrefs.GetFloat("bestScore4");
        score5.text = "5 - " + PlayerPrefs.GetFloat("bestScore5");
    }

    public void InitializePlayerPref()
    {
        if (!PlayerPrefs.HasKey("bestScore1"))
        {
            PlayerPrefs.SetFloat("bestScore1", 0);
        }
        
        if (!PlayerPrefs.HasKey("bestScore2"))
        {
            PlayerPrefs.SetFloat("bestScore2", 0);
        }
        
        if (!PlayerPrefs.HasKey("bestScore3"))
        {
            PlayerPrefs.SetFloat("bestScore3", 0);
        }
        
        if (!PlayerPrefs.HasKey("bestScore4"))
        {
            PlayerPrefs.SetFloat("bestScore4", 0);
        }
        
        if (!PlayerPrefs.HasKey("bestScore5"))
        {
            PlayerPrefs.SetFloat("bestScore5", 0);
        }
    }
}
