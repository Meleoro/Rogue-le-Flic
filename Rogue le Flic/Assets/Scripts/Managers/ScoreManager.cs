using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    
    public TMP_Text scoreActuelText;
    public TMP_Text scoreCountText;
    //public TMP_Text timerText;
    public TMP_Text enemyKillTxt;



    public int scoreActuel;
    public int scoreCount;
    public int enemyKill;
    public float timer;
    public int scoreAdd = 100;
    public bool timerTrue;
    public int valeurMax;
    
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
        //timerText.alpha = 0;
        enemyKill = 1;
        timer = timerMax;
        timerTrue = true;
    }


    private void Update()
    {
        scoreCountText.text = "+" + scoreCount.ToString();
        scoreActuelText.text = scoreActuel.ToString();
        enemyKillTxt.text = "x" + enemyKill.ToString();
        

        timer = timer - Time.deltaTime;


        
        if (Input.GetKeyDown(KeyCode.K))
        {
            EnemyKilled();
        }


        if (timer < 0 && timerTrue == false)
        {
            NoTimeLeft();
        }
        Debug.Log(timer);
    }

    public void EnemyKilled()
    {
        scoreCountText.alpha = 255;
        //timerText.alpha = 255;
        scoreCount = scoreCount + scoreAdd*enemyKill;
        timer = timerMax;
        timerTrue = false;
        enemyKill = enemyKill + 1;
        enemyKillTxt.alpha = 255;
        enemyKillTxt.DOFade(0, timerMax);
        scoreCountText.DOFade(0, timerMax);
    }


    public void NoTimeLeft()
    {
        //valeurMax = scoreCount;
        
        for (int i = 0; i < scoreCount; i++)
        {
            StartCoroutine(Tututu());
        }
        timerTrue = true;
        scoreCount = 0;
        enemyKill = 1;
        //scoreCountText.alpha = 0;
        //enemyKillTxt.alpha = 0;
        //timerText.alpha = 0;
    }


    IEnumerator Tututu()
    {
        scoreActuel = scoreActuel + 1;
        yield return new WaitForSeconds(0.1f);
    }
}
