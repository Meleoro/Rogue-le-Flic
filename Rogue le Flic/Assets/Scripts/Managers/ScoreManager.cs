using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    
    public TMP_Text scoreText;
    public TMP_Text highscoreText;
    
    
    private int score = 0;
    private int highscore = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = score.ToString() + " Points";
        highscoreText.text = "Highscore: " + highscore.ToString();
        Debug.Log("score =" + score);

    }

    public void AddPoint()
    {
        score += 100;
        scoreText.text = score.ToString() + " Points";
        Debug.Log("score =" + score);
    }
    
}
