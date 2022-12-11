using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    
    public TMP_Text scoreActuelText;
    public TMP_Text scoreCountText;
    public TMP_Text timerText;
    public TMP_Text enemyKillTxt;



    public int scoreActuel;
    public int scoreCount;
    public int enemyKill;
    public float timer;
    public int scoreAdd = 100;

    public int timerMax = 5;
    
    

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreActuel = 0;
        scoreCountText.alpha = 0;
        enemyKillTxt.alpha = 0;
        timerText.alpha = 0;
        enemyKill = 1;
        timer = timerMax;
    }


    private void Update()
    {
        scoreCountText.text = scoreCount.ToString();
        scoreActuelText.text = scoreActuel.ToString();
        enemyKillTxt.text = enemyKill.ToString();
        

        timer = timer - 0.005f;
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            EnemyKilled();
        }


        if (timer < 0)
        {
            NoTimeLeft();
        }
        Debug.Log(timer);
    }

    public void EnemyKilled()
    {
        scoreCountText.alpha = 255;
        timerText.alpha = 255;
        scoreCount = scoreCount + scoreAdd*enemyKill;
        timer = 5;
        enemyKill = enemyKill + 1;
        enemyKillTxt.alpha = 255;

    }


    public void NoTimeLeft()
    {
        scoreActuel = scoreActuel + scoreCount;
        scoreCount = 0;
        scoreCountText.alpha = 0;
        timerText.alpha = 0;
    }

}
