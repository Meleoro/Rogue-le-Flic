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


    public AudioSource enemykilled;
    
    
    

    private void Awake()
    {
        if(instance == null)
            instance = this;
        
        else
            Destroy(gameObject);
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
        
        // SAUVEGARDE DES SCORES
        InitializePlayerPref();
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
        
        
        Debug.Log(enemyKill);
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

        scoreActuel += 100;
        
        
        
        

        enemykilled.Play();
        
        enemykilled.pitch = 0.5f*enemyKill;

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
    
    public void ActualiseScores()
    {
        if (PlayerPrefs.GetFloat("bestScore1") < ScoreManager.instance.scoreActuel)
        {
            PlayerPrefs.SetFloat("bestScore1", ScoreManager.instance.scoreActuel);
        }
        
        else if (PlayerPrefs.GetFloat("bestScore2") < ScoreManager.instance.scoreActuel)
        {
            PlayerPrefs.SetFloat("bestScore2", ScoreManager.instance.scoreActuel);
        }
        
        else if (PlayerPrefs.GetFloat("bestScore3") < ScoreManager.instance.scoreActuel)
        {
            PlayerPrefs.SetFloat("bestScore3", ScoreManager.instance.scoreActuel);
        }
        
        else if (PlayerPrefs.GetFloat("bestScore4") < ScoreManager.instance.scoreActuel)
        {
            PlayerPrefs.SetFloat("bestScore4", ScoreManager.instance.scoreActuel);
        }
        
        else if (PlayerPrefs.GetFloat("bestScore5") < ScoreManager.instance.scoreActuel)
        {
            PlayerPrefs.SetFloat("bestScore5", ScoreManager.instance.scoreActuel);
        }
    }
}
