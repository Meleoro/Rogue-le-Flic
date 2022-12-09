using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject player;
    [SerializeField] GameObject settPanel;
    public bool isPaused;
    public bool settOpen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel && !isPaused)
            {
                OpenPause();
                isPaused = true;

                //désactiver le script pour faire bouger le personnage
                
                //Les lignes ci-dessous ferment le menu pause en même temps que la page d'options

                if (Input.GetKeyDown(KeyCode.Escape) && settOpen)
                {
                    CloseSett();
                    settOpen = false;
                }
            }
            
            else
            {
                ClosePause();
                CloseSett();
                isPaused = false;
                settOpen = false;

                //réactiver le script du personnage
            }

        }
    }

    public void OpenPause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ClosePause()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    //Ouvrir les options
    public void OpenSett()
    {
        settPanel.SetActive(true);
        settOpen = true;
    }
    
    //Fermer les options
    public void CloseSett()
    {
        settPanel.SetActive(false);
        settOpen = false;
    }

    //La fonction ReturnToMain() ne marche pas parce que la scene n'est pas dans les Build Settings
    public void ReturnToMain()
    {
        SceneManager.LoadScene("MAIN MENU");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
