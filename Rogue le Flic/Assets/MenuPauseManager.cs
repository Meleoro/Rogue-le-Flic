using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;


public class MenuPauseManager : MonoBehaviour
{
    public static MenuPauseManager Instance;
    
    public GameObject menuPause;

    private bool pausedGame;
    [HideInInspector] public bool otherMenuActive;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);
    }


    private void Start()
    {
        menuPause.GetComponent<Image>().DOFade(0.5f, 0);
        menuPause.SetActive(false);

        pausedGame = false;
    }



    private void Update()
    {
        if (!otherMenuActive)
        {
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                if (pausedGame)
                {
                    ResumeGame();
                }

                else
                {
                    PauseGame();
                }
            }
        }
    }



    public void PauseGame()
    {
        Time.timeScale = 0;
        pausedGame = true;

        menuPause.GetComponent<Image>().DOFade(0.5f, 0);

        Viseur.Instance.viseurActif = false;
        ManagerChara.Instance.noControl = true;

        menuPause.SetActive(true);

        CameraMovements.Instance.canMove = false;
    }


    public void ResumeGame()
    {
        Viseur.Instance.viseurActif = true;
        ManagerChara.Instance.noControl = false;
        
        Time.timeScale = 1;
        pausedGame = false;

        menuPause.GetComponent<Image>().DOFade(0.5f, 0f);

        menuPause.SetActive(false);

        CameraMovements.Instance.canMove = true;
    }


    public void Restart()
    {
        Time.timeScale = 1;

        StartCoroutine(FonduManager.Instance.ChangeScene("Level 1", true));
    }


    public void MenuPrincipal()
    {
        Time.timeScale = 1;

        StartCoroutine(FonduManager.Instance.ChangeScene("MAIN MENU", true));
    }
}
