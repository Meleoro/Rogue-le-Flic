using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HeightSortingChara : MonoBehaviour
{
    private SortingGroup sortingGroup;
    

    private void Start()
    {
        sortingGroup = GetComponent<SortingGroup>();
    }


    void Update()
    {
        if(!ReferenceCamera.Instance.finalCinematic)
            sortingGroup.sortingOrder = Mathf.RoundToInt(transform.position.y * 2) * -1;

        else
            sortingGroup.sortingOrder = 30003;
    }
}
