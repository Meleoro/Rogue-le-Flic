using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Box : MonoBehaviour
{
    [SerializeField] private GameObject fragment;
    [SerializeField] private int nbrFragments;

    private void OnCollisionEnter2D(Collision2D col)
    {
        Explose();
    }

    public void Explose()
    {
        for (int k = 0; k < nbrFragments; k++)
        {
            Vector3 basePos = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.1f, 0.5f));
            
            GameObject currentFragment = Instantiate(fragment, transform.position + basePos, Quaternion.identity);

            if(Random.Range(0, 2) == 0)
                currentFragment.GetComponent<Fragment>().goLeft = true;
        }
        
        Destroy(gameObject);
    }
}
