using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;


public class IntroManager : MonoBehaviour
{
    public Animator anim;

    public TextMeshProUGUI tip;

    public Image fondu;

    private float timer;
    private bool milieu;
    private bool fin;

    private float timer2;
    private bool doOnce;


    public AudioSource switchOn;
    public AudioSource switchOff;
    public AudioSource handStomp;
    public AudioSource paper;
    

    private void Start()
    {
        fondu.DOFade(0, 2);
    }


    private void Update()
    {
        if (timer < 3 && !milieu)
        {
            timer += Time.deltaTime;
        }

        else if (Input.GetKeyDown(KeyCode.Mouse0) && !milieu)
        {
            milieu = true;
            anim.SetTrigger("milieu");
            
            switchOn.Play();

            timer = 0;
        }

        else if (!milieu)
        {
            timer2 += Time.deltaTime;

            if(timer2 > 3)
            {
                tip.DOFade(1, 5);
            }
        }


        if(milieu && timer < 1 && !fin)
        {
            timer2 = 0;

            DOTween.Kill(tip);
            tip.DOFade(0, 0.5f);

            timer += Time.deltaTime;
        }

        else if(milieu && Input.GetKeyDown(KeyCode.Mouse0) && !fin)
        {
            anim.SetTrigger("fin");
            
            handStomp.Play();
            
            fin = true;

            timer = 0;        
        }

        else if (!fin)
        {
            timer2 += Time.deltaTime;

            if (timer2 > 10)
            {
                tip.DOFade(1, 5);
            }
        }



        if (timer < 3 && fin)
        {
            DOTween.Kill(tip);
            tip.DOFade(0, 0.5f);

            timer += Time.deltaTime;
        }

        else if(timer < 4 && fin)
        {
            timer += Time.deltaTime;

            if (!doOnce)
            {
                fondu.DOFade(1, 1);
                doOnce = true;
            }
        }

        else if (fin)
        {
            SceneManager.LoadScene("MAIN MENU");
        }
    }
}
