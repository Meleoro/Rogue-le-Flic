using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;


public class MenuPauseManager : MonoBehaviour
{
    public GameObject menuPause;

    private bool pausedGame;



    private void Start()
    {
        menuPause.GetComponent<Image>().DOFade(0.5f, 0);
        menuPause.SetActive(false);

        pausedGame = false;
    }



    private void Update()
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



    public void PauseGame()
    {
        Time.timeScale = 0;
        pausedGame = true;

        menuPause.GetComponent<Image>().DOFade(0.5f, 0);

        menuPause.SetActive(true);

        CameraMovements.Instance.canMove = false;
    }


    public void ResumeGame()
    {
        Time.timeScale = 1;
        pausedGame = false;

        menuPause.GetComponent<Image>().DOFade(0.5f, 0f);

        menuPause.SetActive(false);

        CameraMovements.Instance.canMove = true;
    }


    public void Restart()
    {
        Time.timeScale = 1;

        Destroy(DontDestroyOnLoadScript2.Instance.gameObject);

        SceneManager.LoadScene("Level 1");
    }


    public void MenuPrincipal()
    {
        Time.timeScale = 1;

        Destroy(DontDestroyOnLoadScript2.Instance.gameObject);

        SceneManager.LoadScene("MAIN MENU");
    }
}
