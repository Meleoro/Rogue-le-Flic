using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipalManager : MonoBehaviour
{
    public Camera _camera;

    [Header("Menu1")] 
    public GameObject buttons;
    public GameObject start;
    public GameObject options;
    public GameObject exit;
    public Button startButton;
    public Button optionButton;
    public Button exitButton;

    [Header("Menu2")] 
    public GameObject menu2;
    
    
    public void Quit()
    {
        Application.Quit();  
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Level 1");
        //Ici faire une référence à ClosePause();
    }

    public void OpenOther()
    {
        options.GetComponent<Animator>().SetTrigger("reset");
        
        start.GetComponent<Animator>().enabled = false;
        options.GetComponent<Animator>().enabled = false;
        exit.GetComponent<Animator>().enabled = false;
        
        buttons.transform.DOLocalMoveY(buttons.transform.position.y + 16, 2).SetEase(Ease.InOutBack);
        start.transform.DOLocalMoveY(start.transform.position.y + 16, 2).SetEase(Ease.InOutBack);
        options.transform.DOMoveY(options.transform.position.y + 16, 2).SetEase(Ease.InOutBack);
        exit.transform.DOMoveY(exit.transform.position.y + 16, 2).SetEase(Ease.InOutBack);
        
        menu2.transform.DOMoveY(menu2.transform.position.y + 16, 2).SetEase(Ease.InOutBack);

        startButton.enabled = false;
        optionButton.enabled = false;
        exitButton.enabled = false;

        StartCoroutine(Zoom(_camera.orthographicSize));
    }

    public void CloseOther()
    {
        buttons.transform.DOLocalMoveY(buttons.transform.position.y - 16, 2).SetEase(Ease.InOutBack);
        start.transform.DOLocalMoveY(start.transform.position.y - 16, 2).SetEase(Ease.InOutBack);
        options.transform.DOMoveY(options.transform.position.y - 16, 2).SetEase(Ease.InOutBack);
        exit.transform.DOMoveY(exit.transform.position.y - 16, 2).SetEase(Ease.InOutBack);
        
        menu2.transform.DOMoveY(menu2.transform.position.y - 16, 2).SetEase(Ease.InOutBack);
        
        startButton.enabled = true;
        optionButton.enabled = true;
        exitButton.enabled = true;

        StartCoroutine(Zoom(_camera.orthographicSize));

        StartCoroutine(ReactivateAnimations());
    }

    IEnumerator ReactivateAnimations()
    {
        yield return new WaitForSeconds(2);
        
        start.GetComponent<Animator>().enabled = true;
        options.GetComponent<Animator>().enabled = true;
        exit.GetComponent<Animator>().enabled = true;
    }

    IEnumerator Zoom(float size)
    {
        _camera.DOOrthoSize(size + 3, 1);
        
        yield return new WaitForSeconds(1);
        
        _camera.DOOrthoSize(size, 1);
    }
}
