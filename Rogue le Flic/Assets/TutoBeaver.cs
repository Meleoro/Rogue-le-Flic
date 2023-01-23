using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoBeaver : MonoBehaviour
{

    public AudioSource stomp;
    
    
    // Start is called before the first frame update
    void Start()
    {
        stomp.PlayDelayed(2.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
