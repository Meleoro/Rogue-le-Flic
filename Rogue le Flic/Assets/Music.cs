using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Music : MonoBehaviour
{


    public bool inCombat = false;
    


public AudioSource musiqueExplo;
public AudioSource musiqueCombat;
public AudioSource musiqueBoss;

    // Start is called before the first frame update
    void Start()
    {
        musiqueExplo.Play();
        musiqueCombat.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (inCombat == false)
            {
                inCombat = true;
            }
            else
            {
                inCombat = false;

            }

        }


        if (inCombat == true)
        {
            musiqueExplo.DOFade(0, 1);
            musiqueCombat.DOFade(1, 1);

        }


        if (inCombat == false)
        {
            musiqueExplo.DOFade(1, 1);
            musiqueCombat.DOFade(0, 1);
        }
    }
}
