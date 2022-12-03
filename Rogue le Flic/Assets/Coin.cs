using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    [Header("Ejection")] 
    [SerializeField] private float ejectionDuration;
    [SerializeField] private float ejectionSpeed;
    private bool isEjecting;
    private float timerEjection;
    private Vector2 originalPos;
    [SerializeField] private AnimationCurve posX;
    [SerializeField] private AnimationCurve posY;
    private float speedX;
    private float speedY;

    [Header("Behavior")] 
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float floatDuration;
    private bool goDown;
    private bool goUp;
    [HideInInspector] public bool isMagneted;
    
    [Header("References")] 
    [SerializeField] private GameObject sprite;


    private void Start()
    {
        goUp = true;

        originalPos = transform.position;
        isEjecting = true;

        speedX = Random.Range(2f, 4f);
        speedY = Random.Range(2f, 3f);
    }


    private void Update()
    {
        if (!isMagneted)
        {
            if (!isEjecting)
            {
                CoinFloat();
                RotationPiece();
            }

            else
            {
                Ejection();
                RotationPiece();
            }
        }

        else
        {
            isEjecting = false;
            DOTween.Kill(gameObject);
        }
    }


    private void Ejection()
    {
        timerEjection += Time.deltaTime * ejectionSpeed;

        transform.position =  originalPos + new Vector2(posX.Evaluate(timerEjection) * speedX, posY.Evaluate(timerEjection) * speedY);

        if (timerEjection >= ejectionDuration)
        {
            isEjecting = false;
        }
    }


    private void RotationPiece()
    {
        sprite.transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
    }

    private void CoinFloat()
    {
        if (!goDown && goUp)
        {
            transform.DOMoveY(sprite.transform.position.y + 0.2f, floatDuration).OnComplete(() => goDown = true).SetEase(Ease.InOutQuad);
            goUp = false;
        }

        else if(goDown && !goUp)
        {
            transform.DOMoveY(sprite.transform.position.y - 0.2f, floatDuration).OnComplete(() => goUp = true).SetEase(Ease.InOutQuad);
            goDown = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        
    }
}
