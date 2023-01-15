using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExplicationUI : MonoBehaviour
{
    public TextMeshProUGUI nom;
    public TextMeshProUGUI description;
    
    
    void Start()
    {
        nom.text = GetComponentInParent<Module>().itemName;
        description.text = GetComponentInParent<Module>().itemDescription;
    }
}
