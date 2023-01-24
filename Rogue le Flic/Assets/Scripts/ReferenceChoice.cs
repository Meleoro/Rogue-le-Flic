using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceChoice : MonoBehaviour
{
    public static ReferenceChoice Instance;

    public RectTransform spare;
    public RectTransform kick;

    public bool kicked;
    public bool spared;

    public GameObject boss;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);
    }

    public void Kicked()
    {
        StartCoroutine(boss.GetComponent<Boss>().EndCinematicDeath(true));
        kicked = true;
    }

    public void Spared()
    {
        StartCoroutine(boss.GetComponent<Boss>().EndCinematicDeath(false));
        spared = true;
    }
}
