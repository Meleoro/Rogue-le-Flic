using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;
    
    [SerializeField] private List<GameObject> hearts = new List<GameObject>();
    private int currentHealth;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentHealth = hearts.Count;
    }

    public void LoseHealth()
    {
        currentHealth -= 1;
        
        hearts[currentHealth].SetActive(false);
    }
}
