using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] GameObject settPanel;
    [SerializeField] GameObject controlsPanel;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && settPanel)
        {
            CloseSettings();
            CloseControls();
        }
    }
    public void Quit()
    {
      Application.Quit();  
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Level 1");
        //Ici faire une référence à ClosePause();
    }

    public void OpenSettings()
    {
        settPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settPanel.SetActive(false);
    }

    public void OpenControls()
    {
        controlsPanel.SetActive(true);
    }

    public void CloseControls()
    {
        controlsPanel.SetActive(false);
    }
}
