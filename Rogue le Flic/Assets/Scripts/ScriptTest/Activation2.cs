using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activation2 : MonoBehaviour
{

    public GameObject gameobject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            gameobject.SetActive(true);
        }
    }
}
