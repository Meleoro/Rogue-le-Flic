using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightSorting : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        _spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * 2) * -1;
    }
}
