using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class OutroManager : MonoBehaviour
{
    public Image fondu;
    public TextMeshProUGUI tip;

    public GameObject imageLampeAllumee;
    public GameObject imageLampeEteinte;
    public GameObject imageCredits;

    public List<Image> imagesCredits;

    private float timer;
    private bool milieu;
    private bool fin;

    private float timer2;

    private bool doOnce;


    private void Start()
    {
        fondu.DOFade(0, 1f);

        doOnce = false;
    }



    private void Update()
    {
        if(timer < 1 && !milieu)
        {
            timer += Time.deltaTime;
        }

        else if(Input.GetKeyDown(KeyCode.Mouse0) && !milieu)
        {
            milieu = true;
            imageLampeAllumee.SetActive(true);

            timer = 0;
        }

        else if (!milieu)
        {
            timer2 += Time.deltaTime;

            if (timer2 > 3)
            {
                tip.DOFade(1, 5);
            }
        }



        if (milieu && timer < 1 && !fin)
        {
            timer2 = 0;

            DOTween.Kill(tip);
            tip.DOFade(0, 0.5f);

            timer += Time.deltaTime;
        }

        else if (milieu && Input.GetKeyDown(KeyCode.Mouse0) && !fin)
        {
            imageLampeEteinte.SetActive(true);

            imageCredits.SetActive(true);
            imageCredits.GetComponent<Image>().DOFade(0, 0);

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



        if (fin && timer < 1)
        {
            timer2 = 0;

            DOTween.Kill(tip);
            tip.DOFade(0, 0.5f);

            timer += Time.deltaTime;
        }

        else if(timer < 8 && fin)
        {
            if (!doOnce)
            {
                imageCredits.GetComponent<Image>().DOFade(1, 3);
                doOnce = true;
            }

            timer += Time.deltaTime;
        }


        if((timer >= 20 && fin) || (Input.GetKeyDown(KeyCode.Mouse0) && timer > 1 && fin))
        {
            fondu.DOFade(1, 1);

            StartCoroutine(Fin());
        }
    }


    IEnumerator Fin()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("MAIN MENU");
    }
}
