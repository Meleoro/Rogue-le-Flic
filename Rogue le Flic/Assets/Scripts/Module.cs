using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Module : MonoBehaviour
{
    private bool canBeGrab;

    public int numberEffect;

    [SerializeField] GameObject UIExplications;
    [SerializeField] GameObject UIChoix;

    [Header("Choix")] 
    [SerializeField] private TextMeshProUGUI moduleName1;
    [SerializeField] private TextMeshProUGUI description1;
    [SerializeField] private Image image1;
    [SerializeField] private TextMeshProUGUI moduleName2;
    [SerializeField] private TextMeshProUGUI description2;
    [SerializeField] private Image image2;

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

        moduleName1.text = ModuleManager.Instance.moduleName1;
        description1.text = ModuleManager.Instance.description1;
        image1.sprite = ModuleManager.Instance.image1;
        
        moduleName2.text = ModuleManager.Instance.moduleName2;
        description2.text = ModuleManager.Instance.description2;
        image2.sprite = ModuleManager.Instance.image2;

        ManagerChara.Instance.noControl = true;

        CameraMovements.Instance.canMove = false;
    }
    
    public void ChoiceSlot(int slot)
    {
        CameraMovements.Instance.canMove = true;

        if (slot == 1)
        {
            ModuleManager.Instance.Module1 = numberEffect;

            ModuleManager.Instance.moduleName1 = itemName;
            ModuleManager.Instance.description1 = itemDescription;
            ModuleManager.Instance.image1 = GetComponent<SpriteRenderer>().sprite;
        }

        else
        {
            ModuleManager.Instance.Module2 = numberEffect;
            
            ModuleManager.Instance.moduleName2 = itemName;
            ModuleManager.Instance.description2 = itemDescription;
            ModuleManager.Instance.image2 = GetComponent<SpriteRenderer>().sprite;
        }
        
        
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
