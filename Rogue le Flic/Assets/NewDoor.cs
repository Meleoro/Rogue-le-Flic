using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDoor : MonoBehaviour
{
    public int doorNumber;
    public bool isFinalDoor;

    public GameObject lightRed;
    public GameObject lightGreen;

    public bool isOpen;

    public BoxCollider2D boxCollider2D;


    private void Update()
    {
        if (isOpen)
        {
            lightRed.SetActive(false);
            lightGreen.SetActive(true);

            boxCollider2D.enabled = true;
        }

        else
        {
            lightRed.SetActive(true);
            lightGreen.SetActive(false);
            
            boxCollider2D.enabled = false;
        }
    }


    /*private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && !isFinalDoor)
        {
            MapManager.Instance.ChangeRoom(doorNumber);
        }

        else if (col.gameObject.CompareTag("Player"))
        {
            LevelManager.Instance.ChangeScene();
        }
    }*/
}
