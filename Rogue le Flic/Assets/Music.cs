using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music Instance;

    public AudioSource musiqueTuto;
public AudioSource musiqueCombat;
public AudioSource musiqueBoss;


private void Awake()
{
    if (Instance == null)
        Instance = this;
    
    else
        Destroy(gameObject);
}


// Start is called before the first frame update
    void Start()
    {
        musiqueTuto.Play();
        musiqueCombat.Play();
        musiqueBoss.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        



    }


    public void TutoACombat()
    {
        musiqueTuto.DOFade(0, 1);
        musiqueCombat.DOFade(1, 1);
        Debug.Log("Tuto = 0");
        Debug.Log("Combat = 1");
    }


    public void CombatABoss()
    {
        musiqueCombat.DOFade(0, 1);
        musiqueBoss.DOFade(1, 1);

        Debug.Log("Combat = 0");
        Debug.Log("Boss = 1");
    }

    public void BossACombat()
    {
        musiqueBoss.DOFade(0, 1);
        musiqueCombat.DOFade(1, 1);

        Debug.Log("Boss = 0");
        Debug.Log("Combat = 1");
    }
}
