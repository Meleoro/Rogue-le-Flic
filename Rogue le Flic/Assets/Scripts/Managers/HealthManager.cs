using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;
    
    [SerializeField] private List<GameObject> hearts = new List<GameObject>();
    private int currentHealth;
    private int maxHealth;
    private bool isInvincible;
    [SerializeField] private float invincibleTime;
    private float timerInvincible;
    public bool immortel;
    public Sprite vie;

    [Header("Feedback Hit")] 
    [SerializeField] private Volume volume;
    [SerializeField] private float speedEffects;
    [SerializeField] private float reculForce;
    [SerializeField] private float shakeAmplitude;
    [SerializeField] private float shakeDuration;
    private float timerEffects;

    public AudioSource damagetaken;
    

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    private void Start()
    {
        currentHealth = hearts.Count;
        maxHealth = hearts.Count;
    }
    

    private void Update()
    {
        if (timerEffects > 0)
            HitEffect();

        if (timerInvincible > 0)
            timerInvincible -= Time.deltaTime;

        else
            isInvincible = false;

    }
    

    public void LoseHealth(Vector2 direction)
    {
        
        if (!isInvincible && currentHealth > 0 && !ManagerChara.Instance.isDashing && !immortel)
        {
            
            
            damagetaken.Play();
            
            
            currentHealth -= 1;

            if(currentHealth % 2 == 1)
            {
                hearts[currentHealth].SetActive(false);
                hearts[currentHealth - 1].SetActive(true);
            }

            else
            {
                hearts[currentHealth].SetActive(false);
            }

            isInvincible = true;

            CameraMovements.Instance.CameraShake(shakeDuration, shakeAmplitude);
                
            ManagerChara.Instance.rb.AddForce(direction.normalized * reculForce, ForceMode2D.Impulse);
        
            timerEffects = 1;

            if (currentHealth <= 0)
            {
                ManagerChara.Instance.noControl = true;

                StartCoroutine(ManagerChara.Instance.Death());
            }

            timerInvincible = invincibleTime;
        }
    }
    

    public void AddHealth()
    {
        currentHealth += 2;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        for (int k = 0; k < currentHealth; k++)
        {
            if (k % 2 == 0)
            {
                hearts[k].SetActive(true);
            }
            else
            {
                hearts[k - 1].SetActive(false);
                hearts[k].SetActive(true);
            }
        }
    }

    public void HitEffect()
    {
        

        
        
        timerEffects -= Time.deltaTime * speedEffects;
        
        if (timerEffects > 0.93f)
        {
            Time.timeScale = 0.1f;
            volume.weight = 1;
        }

        else
        {
            Time.timeScale = Mathf.Lerp(1, 0.6f, timerEffects);
            volume.weight = Mathf.Lerp(0, 1f, timerEffects);
        }

        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
