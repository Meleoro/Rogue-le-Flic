using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public TextMeshPro scoreText; 
    public Text highscoreText;
    public TextMeshPro oziejr;
    

    private int score = 0;
    private int highscore = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = score.ToString() + "Points";
        highscoreText.text = "Highscore: " + highscore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
