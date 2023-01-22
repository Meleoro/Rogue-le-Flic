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

    [Header("BossDoor")]
    public GameObject tagBoss;
    [ColorUsage(true, true)] public Color colorBoss;
    public bool isBossDoor;


    [Header("NextLevelDoor")]
    public GameObject tagNextLevelDoor;
    [ColorUsage(true, true)] public Color colorNext;
    public bool isNextLevelDoor;


    [Header("MaterialPorte")]
    public Material material;
    [ColorUsage(true, true)]  public Color red;
    [ColorUsage(true, true)]  public Color green;


    public AudioSource ding;
    public AudioSource open;
    

    private void Start()
    {
        if (isBossDoor)
        {
            tagBoss.SetActive(true);
            tagNextLevelDoor.SetActive(false);
        }

        else if (isFinalDoor)
        {
            tagBoss.SetActive(false);
            tagNextLevelDoor.SetActive(true);
        }

        else
        {
            tagBoss.SetActive(false);
            tagNextLevelDoor.SetActive(false);
        }
    }



    private void Update()
    {
        if (isOpen)
        {
            if (!isBossDoor)
            {
                lightRed.SetActive(false);
                lightGreen.SetActive(true);

                material.color = green;

                boxCollider2D.enabled = true;
                ding.Play();
            }
            else
            {
                lightRed.SetActive(false);
                lightGreen.SetActive(true);

                material.color = colorBoss;

                boxCollider2D.enabled = true;
                ding.Play();
            }
            

        }

        else
        {
            lightRed.SetActive(true);
            lightGreen.SetActive(false);

            material.color = red;

            boxCollider2D.enabled = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
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
