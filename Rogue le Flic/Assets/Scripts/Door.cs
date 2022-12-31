using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int doorNumber;
    public bool isFinalDoor;


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && !isFinalDoor)
        {
            MapManager.Instance.ChangeRoom(doorNumber);
        }

        else if (col.gameObject.CompareTag("Player"))
        {
            LevelManager.Instance.ChangeScene();
        }
    }
}
