using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fragment : MonoBehaviour
{
    [SerializeField] private AnimationCurve trajectoire;
    [SerializeField] private float vitesseFall;
    public bool goLeft;
    private float timer;

    [Header("Aleatoire")] 
    private float modificateurRotation;
    private float modificateurHauteur;
    private float modificateurX;
    private float modificateurVitesse;

    private Vector3 originalPos;

    private void Start()
    {
        timer = 1;

        originalPos = transform.position;

        modificateurRotation = Random.Range(0, 1.5f);
        modificateurHauteur = Random.Range(-0.15f, 0.15f);
        modificateurX = Random.Range(0.8f, 3f);
        modificateurVitesse = Random.Range(1, 1.3f);
    }

    private void Update()
    {
        if (goLeft)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime * vitesseFall * modificateurVitesse;

                transform.position = originalPos + new Vector3((-1 + timer) / modificateurX, trajectoire.Evaluate(timer) + modificateurHauteur);
            
                transform.Rotate(0, 0, 0.5f + modificateurRotation);
            }
            else
            {
                this.enabled = false;
            } 
        }
        else
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime * vitesseFall * modificateurVitesse;

                transform.position = originalPos + new Vector3((1 - timer) / modificateurX, trajectoire.Evaluate(timer) + modificateurHauteur);
            
                transform.Rotate(0, 0, -0.5f - modificateurRotation);
            }
            else
            {
                this.enabled = false;
            }
        }
    }
}
