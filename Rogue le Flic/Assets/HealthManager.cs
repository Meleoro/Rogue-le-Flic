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

    [Header("Feedback Hit")] 
    [SerializeField] private Volume volume;
    [SerializeField] private float speedEffects;
    [SerializeField] private float reculForce;
    [SerializeField] private float shakeAmplitude;
    [SerializeField] private float shakeDuration;
    private float timerEffects;
    
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentHealth = hearts.Count;
    }

    private void Update()
    {
        if(timerEffects > 0)
            HitEffect();
    }

    public void LoseHealth(Vector2 direction)
    {
        currentHealth -= 1;
        hearts[currentHealth].SetActive(false);

        ReferenceCamera.Instance._camera.DOShakePosition(shakeDuration, shakeAmplitude);
        
        ManagerChara.Instance.rb.AddForce(direction.normalized * reculForce, ForceMode2D.Impulse);
        
        timerEffects = 1;
    }

    public void HitEffect()
    {
        timerEffects -= Time.deltaTime * speedEffects;

        volume.weight = Mathf.Lerp(0, 1, timerEffects);

        if (timerEffects > 0.7f)
            Time.timeScale = 0.1f;

        else
            Time.timeScale = Mathf.Lerp(1, 0.6f, timerEffects);
        
    }
}
