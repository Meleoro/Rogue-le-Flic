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


    public IEnumerator ChangeScene(string sceneName, bool destroy)
    {
        fondu.gameObject.SetActive(true);
        
        fondu.DOFade(1, 0.5f);
        
        yield return new WaitForSeconds(0.5f);

        if(destroy)
            Destroy(DontDestroyOnLoadScript2.Instance.gameObject);

        fondu.DOFade(0, 0.5f);

        SceneManager.LoadScene(sceneName);
        
        yield return new WaitForSeconds(0.5f);

        fondu.gameObject.SetActive(false);
    }

    public void Gestionfondu()
    {
        fondu.DOFade(1, 0.5f)
            .OnComplete((() => fondu.DOFade(0, 0.5f).OnComplete(() => fondu.gameObject.SetActive(false))));
    }
}
