using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using UnityEngine.SceneManagement;

public class FonduManager : MonoBehaviour
{
    public static FonduManager Instance;
    
    public Image fondu;

    public bool fonduActif;

    private float timer;
    private bool doOnce;


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
        
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        fonduActif = false;
    }


    public IEnumerator ChangeScene(string sceneName, bool destroy)
    {
        fondu.gameObject.SetActive(true);
        
        fondu.DOFade(1, 0.5f);
        
        yield return new WaitForSeconds(0.5f);

        if(destroy)
            Destroy(DontDestroyOnLoadScript2.Instance.gameObject);

        fonduActif = true;
        doOnce = false;

        SceneManager.LoadScene(sceneName);
    }

    public void Gestionfondu()
    {
        fondu.DOFade(1, 0.5f)
            .OnComplete((() => fondu.DOFade(0, 0.5f).OnComplete(() => fondu.gameObject.SetActive(false))));
    }


    private void Update()
    {
        if (fonduActif)
        {
            timer += Time.deltaTime;
            
            if(!doOnce)
                fondu.DOFade(0, 0.5f);

            if (timer > 0.5f)
            {
                fonduActif = false;
                fondu.gameObject.SetActive(false);
            }

        }
    }
}
