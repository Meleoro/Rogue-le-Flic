using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    private bool canBeGrab;

    public int numberEffect;

    [SerializeField] GameObject UIExplications;
    [SerializeField] GameObject UIChoix;

    [Header("Shop")] 
    public string itemName;
    public string itemDescription;

    private bool UIActive;
    

    private void Start()
    {
        UIExplications.SetActive(false);
        UIChoix.SetActive(false);
    }


    private void Update()
    {
        if (MovementsChara.Instance.controls.Character.Enter.WasPerformedThisFrame() && canBeGrab)
            OpenChoice();

        else if(UIActive)
            UIChoix.SetActive(true);
    }
    
    public void OpenChoice()
    {
        UIActive = true;
        
        UIExplications.SetActive(false);
        UIChoix.SetActive(true);

        ManagerChara.Instance.noControl = true;
    }
    
    public void ChoiceSlot(int slot)
    {
        if (slot == 1)
            ModuleManager.Instance.Module1 = numberEffect;

        else
            ModuleManager.Instance.Module2 = numberEffect;
        
        
        UIChoix.SetActive(false);
        Destroy(gameObject);
        
        ManagerChara.Instance.noControl = false;
    }



    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            canBeGrab = true;
            UIExplications.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            canBeGrab = false;
            UIExplications.SetActive(false);
        }
    }
}
