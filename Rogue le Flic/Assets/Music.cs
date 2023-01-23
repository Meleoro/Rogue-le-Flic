using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{


public AudioSource musiqueExplo;
public AudioSource musiqueCombat;
public AudioSource musiqueBoss;

    // Start is called before the first frame update
    void Start()
    {
        musiqueExplo.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
