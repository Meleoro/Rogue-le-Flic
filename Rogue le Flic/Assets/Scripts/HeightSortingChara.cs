using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HeightSortingChara : MonoBehaviour
{
    private SortingGroup sortingGroup;

    [SerializeField] private bool isChara;
    [SerializeField] private bool isEnnemi;
    

    private void Start()
    {
        sortingGroup = GetComponent<SortingGroup>();
    }


    void Update()
    {
        if((!ReferenceCamera.Instance.finalCinematic && !ReferenceCamera.Instance.finalCinematicChara) || isEnnemi)
            sortingGroup.sortingOrder = Mathf.RoundToInt(transform.position.y * 2) * -1;

        else if (!isEnnemi)
        {
            if (!isChara && !ReferenceCamera.Instance.finalCinematicChara)
                sortingGroup.sortingOrder = 30005;

            else if (ReferenceCamera.Instance.finalCinematicChara)
                sortingGroup.sortingOrder = 30005;
        }
    }
}
