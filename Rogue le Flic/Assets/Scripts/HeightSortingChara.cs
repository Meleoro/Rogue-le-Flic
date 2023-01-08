using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HeightSortingChara : MonoBehaviour
{
    private SortingGroup sortingGroup;

    [SerializeField] private bool isChara;
    

    private void Start()
    {
        sortingGroup = GetComponent<SortingGroup>();
    }


    void Update()
    {
        if(!ReferenceCamera.Instance.finalCinematic)
            sortingGroup.sortingOrder = Mathf.RoundToInt(transform.position.y * 2) * -1;

        else if (!isChara && !ReferenceCamera.Instance.finalCinematicChara)
            sortingGroup.sortingOrder = 30004;

        else if (ReferenceCamera.Instance.finalCinematicChara)
            sortingGroup.sortingOrder = 30004;
    }
}
