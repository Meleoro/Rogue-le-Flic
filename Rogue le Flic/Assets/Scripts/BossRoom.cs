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
        }
        
        StartCoroutine(LaunchRoom());
    }

    IEnumerator LaunchRoom()
    {
        if (boss2 is not null)
        {
            boss2.GetComponent<Boss>().canMove = false;
        }
        if (boss3 is not null)
        {
            boss3.GetComponent<Boss>().canMove = false;
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
        
        yield return new WaitForSeconds(3);
        
        ReferenceCamera.Instance.splash.SetActive(false);
        
        
        CameraMovements.Instance.posCamera = ManagerChara.Instance.transform.position;
        CameraMovements.Instance.timeZoom = 0.2f;
        CameraMovements.Instance.Reboot();

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
}
