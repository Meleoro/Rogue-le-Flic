using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public List<Transform> spotFrog;

    public bool isLastBoss;

    public GameObject boss;

    private GameObject boss2;
    private GameObject boss3;

    private int boss2Int;
    private int boss3Int;


    public AudioSource sweep;
    

    private void Start()
    {
        if (isLastBoss)
        {
            for (int k = 0; k < LevelManager.Instance.savedBoss.Count; k++)
            {
                if(k == 0)
                    boss2 = Instantiate(LevelManager.Instance.savedBoss[k], transform.position + new Vector3(3, 0, 0), Quaternion.identity, transform);
                
                else if (k == 1)
                    boss3 = Instantiate(LevelManager.Instance.savedBoss[k], transform.position + new Vector3(-3, 0, 0), Quaternion.identity, transform);

                
                if (k == 0)
                    boss2.GetComponent<Boss>().bossNumber = k + 1;
                
                else if (k == 1)
                    boss3.GetComponent<Boss>().bossNumber = k + 1;
            }

            if(LevelManager.Instance.savedBoss.Count == 0)
            {
                boss.GetComponent<Boss>().isAlone = true;
            }
        }

        boss.GetComponent<Boss>().bossRoom = gameObject;

        StartCoroutine(LaunchRoom());
    }

    IEnumerator LaunchRoom()
    {
        if (boss2 is not null)
        {
            boss2.GetComponent<Boss>().canMove = false;


            switch (boss2.GetComponent<Boss>().bossType)
            {
                case Boss.boss.Beaver:
                    boss2Int = 0;
                    break;

                case Boss.boss.Frog:
                    boss2Int = 1;
                    break;

                case Boss.boss.Turtle:
                    boss2Int = 2;
                    break;
            }
        }

        else
        {
            boss2Int = 3;
        }


        if (boss3 is not null)
        {
            boss3.GetComponent<Boss>().canMove = false;

            switch (boss3.GetComponent<Boss>().bossType)
            {
                case Boss.boss.Beaver:
                    boss3Int = 0;
                    break;

                case Boss.boss.Frog:
                    boss3Int = 1;
                    break;

                case Boss.boss.Turtle:
                    boss3Int = 2;
                    break;
            }
        }

        else
        {
            boss3Int = 3;
        }
        

        CameraMovements.Instance.transform.position = ManagerChara.Instance.transform.position;
        CameraMovements.Instance._camera.orthographicSize = GetComponent<CameraManager>().cameraSize;

        CameraMovements.Instance.bossStartRoom = true;
        CameraMovements.Instance.posCamera = boss.transform.position;
        CameraMovements.Instance.timeZoom = 2;

        ManagerChara.Instance.noControl = true;
        
        boss.GetComponent<Boss>().canMove = false;
        
        yield return new WaitForSeconds(2);

        ReferenceCamera.Instance.splash.SetActive(true);


        boss.GetComponent<Boss>().VerifyBossType(boss2Int, boss3Int);

        sweep.Play();
        
        yield return new WaitForSeconds(3);
        
        ReferenceCamera.Instance.splash.SetActive(false);
        
        
        /*CameraMovements.Instance.posCamera = ManagerChara.Instance.transform.position;
        CameraMovements.Instance.timeZoom = 0.2f;
        CameraMovements.Instance.Reboot();*/

        CameraMovements.Instance.timerTransition = 1;
        CameraMovements.Instance.departTransition = CameraMovements.Instance.transform.position;
        CameraMovements.Instance.isInTransition = true;

        yield return new WaitForSeconds(0.2f);
        
        ManagerChara.Instance.noControl = false;
        boss.GetComponent<Boss>().canMove = true;
        
        CameraMovements.Instance.bossStartRoom = false;
        
        if (boss2 is not null)
        {
            boss2.GetComponent<Boss>().canMove = true;
        }
        if (boss3 is not null)
        {
            boss3.GetComponent<Boss>().canMove = true;
        }
    }



    public void DestroyWhatIsLeft()
    {
        if (boss2 is not null)
        {
            //StartCoroutine(boss2.GetComponent<Boss>().HurtDeath());
            
            Destroy(boss2);
        }
        if (boss3 is not null)
        {
            //StartCoroutine(boss3.GetComponent<Boss>().HurtDeath());
            Destroy(boss3);
        }
    }
}
