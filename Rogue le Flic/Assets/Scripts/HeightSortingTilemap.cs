using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HeightSortingTilemap : MonoBehaviour
{
    private TilemapRenderer _spriteRenderer;
    [SerializeField] private Transform posRef;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<TilemapRenderer>();
        
        _spriteRenderer.sortingOrder = Mathf.RoundToInt(posRef.position.y * 2) * -1;
    }
}
