using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDoor : MonoBehaviour
{
    public int doorNumber;
    public bool isFinalDoor;

    public GameObject lightRed;
    public GameObject lightGreen;
    public GameObject lightBlue;

    public bool isOpen;

    [Header("UpDoor")]
    public bool isUpDoor;
    public Animator anim;

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

    private bool doOnce;
    

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

        doOnce = false;
    }



    private void Update()
    {
        if (isOpen)
        {
            if (isFinalDoor)
            {
                lightRed.SetActive(false);
                lightBlue.SetActive(true);

                material.color = colorNext;

                boxCollider2D.enabled = true;
            }

            else if (!isBossDoor)
            {
                lightRed.SetActive(false);
                lightGreen.SetActive(true);

                material.color = green;

                boxCollider2D.enabled = true;
            }

            else
            {
                lightRed.SetActive(false);
                lightGreen.SetActive(true);

                material.color = green;

                boxCollider2D.enabled = true;
            }

            if (!doOnce)
            {

                if (isUpDoor)
                {
                    anim.enabled = true;
                    anim.SetTrigger("openDoor");
                }

                ding.Play();
                doOnce = true;
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
