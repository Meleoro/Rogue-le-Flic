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
    [Range(1, 3)] public int levelEffect;

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
        
        else if((MovementsChara.Instance.controls.Character.Enter.WasPerformedThisFrame() 
                || Input.GetKeyDown(KeyCode.Escape)) && UIActive)
            CloseChoice();

        else if(UIActive)
            UIChoix.SetActive(true);
    }
    
    public void OpenChoice()
    {
        canBeGrab = false;
        
        UIActive = true;
        MenuPauseManager.Instance.otherMenuActive = true;
        
        UIExplications.SetActive(false);
        UIChoix.SetActive(true);

        if (ModuleManager.Instance.Module1 != 0)
        {
            image1.enabled = true;
        }
        else
        {
            image1.enabled = false;
        }
        
        if (ModuleManager.Instance.Module2 != 0)
        {
            image2.enabled = true;
        }
        else
        {
            image2.enabled = false;
        }

        moduleName1.text = ModuleManager.Instance.moduleName1;
        description1.text = ModuleManager.Instance.description1;
        image1.sprite = ModuleManager.Instance.image1;
        
        moduleName2.text = ModuleManager.Instance.moduleName2;
        description2.text = ModuleManager.Instance.description2;
        image2.sprite = ModuleManager.Instance.image2;

        ManagerChara.Instance.noControl = true;

        CameraMovements.Instance.canMove = false;
    }

    public void CloseChoice()
    {
        UIExplications.SetActive(true);
        UIChoix.SetActive(false);

        UIActive = false;
        
        ManagerChara.Instance.noControl = false;
        CameraMovements.Instance.canMove = true;
        
        MenuPauseManager.Instance.otherMenuActive = true;

        CameraMovements.Instance.departTransition = CameraMovements.Instance.transform.position;
        CameraMovements.Instance.timerTransition = 1;
        CameraMovements.Instance.isInTransition = true;

        canBeGrab = true;
    }
    
    public void ChoiceSlot(int slot)
    {
        CameraMovements.Instance.timerTransition = 1;
        CameraMovements.Instance.isInTransition = true;
        CameraMovements.Instance.departTransition = CameraMovements.Instance.transform.position;
        
        MenuPauseManager.Instance.otherMenuActive = true;
        CameraMovements.Instance.canMove = true;

        if (slot == 1)
        {
            if(ModuleManager.Instance.Module1 != 0)
            {
                Instantiate(ModuleManager.Instance.objectModule1, transform.position, Quaternion.identity);
                Destroy(ModuleManager.Instance.objectModule1);
            }
            
            ModuleManager.Instance.objectModule1 = Instantiate(gameObject, new Vector3(-10000, -10000, 0), Quaternion.identity, LevelManager.Instance.transform);
            
            ModuleManager.Instance.Module1 = numberEffect;
            ModuleManager.Instance.levelModule1 = levelEffect;

            ModuleManager.Instance.moduleName1 = itemName;
            ModuleManager.Instance.description1 = itemDescription;
            ModuleManager.Instance.image1 = GetComponent<SpriteRenderer>().sprite;
        }

        else
        {
            if(ModuleManager.Instance.Module2 != 0)
            {
                Instantiate(ModuleManager.Instance.objectModule2, transform.position, Quaternion.identity);
                Destroy(ModuleManager.Instance.objectModule2);
            }
            
            ModuleManager.Instance.objectModule2 = Instantiate(gameObject, new Vector3(-10000, -10000, 0), Quaternion.identity, LevelManager.Instance.transform);
            
            ModuleManager.Instance.Module2 = numberEffect;
            ModuleManager.Instance.levelModule2 = levelEffect;
            
            ModuleManager.Instance.moduleName2 = itemName;
            ModuleManager.Instance.description2 = itemDescription;
            ModuleManager.Instance.image2 = GetComponent<SpriteRenderer>().sprite;
        }
        
        
        UIChoix.SetActive(false);
        Destroy(gameObject);
        
        ManagerChara.Instance.noControl = false;

        ModuleManager.Instance.ActualiserHUD();
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
