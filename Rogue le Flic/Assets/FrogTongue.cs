using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Collections;

public class FrogTongue : MonoBehaviour
{
    public Vector2 destination;
    public Vector2 retour;
    public float tongueDuration;

    private float avancée;

    private Rigidbody2D rb;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();


        Vector3 direction = ManagerChara.Instance.transform.position - transform.position;
        destination = ManagerChara.Instance.transform.position + direction.normalized * 2;

        transform.DOMove(destination, tongueDuration - 0.5f);

        retour = transform.position;
    }

    private void Update()
    {
        avancée += Time.deltaTime;

        if (avancée >= tongueDuration - 0.5f)
        {
            transform.DOMove(retour, 0.5f);
        }

        if (avancée >= tongueDuration)
        {
            DOTween.CompleteAll();
            
            Destroy(gameObject);
        }
    }
}
