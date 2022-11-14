using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    private bool canBeGrab;

    public int numberEffect;

    public GameObject UI;


    private void Start()
    {
        UI.SetActive(false);
    }


    private void Update()
    {
        if (MovementsChara.Instance.controls.Character.Module1.WasPerformedThisFrame() && canBeGrab)
        {
            ModuleManager.Instance.Module1 = numberEffect;
            
            Destroy(gameObject);
        }

        else if (MovementsChara.Instance.controls.Character.Module2.WasPerformedThisFrame() && canBeGrab)
        {
            ModuleManager.Instance.Module2 = numberEffect;
            
            Destroy(gameObject);
        }
    }



    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            canBeGrab = true;
            UI.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            canBeGrab = false;
            UI.SetActive(false);
        }
    }
}
