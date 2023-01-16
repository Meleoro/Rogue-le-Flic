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
        SceneManager.LoadScene("Tuto");
        //Ici faire une référence à ClosePause();
    }

    public void OpenSettings()
    {
        
    }

    public void CloseSettings()
    {
        
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
