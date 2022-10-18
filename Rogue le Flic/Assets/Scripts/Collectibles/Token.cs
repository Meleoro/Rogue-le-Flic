using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Token : MonoBehaviour
{
    //public AudioClip sound;
    public int currentTokenCount;
    
    private void Start()
    {
        currentTokenCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ChangeNumber());
            //currentTokenCount = currentTokenCount + 1;
            //GetComponent<SpriteRenderer>().enabled = false;
            //GetComponent<BoxCollider2D>().enabled = false;
            //AudioManager.instance.PlayClipAt(sound, transform.position);
        }
    }
    IEnumerator ChangeNumber()
    {
        yield return new WaitForSeconds(0.01f);
        CounterToken.instance.AddCounterToken(1);
        Destroy(gameObject,0.00001f);
    }

}
