using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausePanel)
            {
                OpenPause();
                Debug.Log("it open");
            }
            else
            {
                ClosePause();
                Debug.Log("it closed");
            }
        }
    }

    public void OpenPause()
    {
        //Ici pause le personnage, les ennemis, les projectiles
        pausePanel.SetActive(true);
    }

    public void ClosePause()
    {
        //Ici dé-pause le personnage et le blabla
        pausePanel.SetActive(false);
    }
}
