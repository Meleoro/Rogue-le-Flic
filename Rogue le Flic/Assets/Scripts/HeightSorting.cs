using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightSorting : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public bool upDoor;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(upDoor)
            _spriteRenderer.sortingOrder = (Mathf.RoundToInt(transform.position.y * 2) * -1) - 1;
        
        else
            _spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * 2) * -1;
    }
}
