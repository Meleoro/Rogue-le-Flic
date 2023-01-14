using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public List<Transform> spawnPoints;

    public bool isLastBoss;

    public GameObject boss;

    private void Start()
    {
        StartCoroutine(LaunchRoom());
        
        if (isLastBoss)
        {
            for (int k = 0; k < LevelManager.Instance.savedBoss.Count; k++)
            {
                GameObject newBoss = null;
                
                if(k == 0)
                    newBoss = Instantiate(LevelManager.Instance.savedBoss[k], transform.position + new Vector3(3, 0, 0), Quaternion.identity);
                
                else if (k == 1)
                    newBoss = Instantiate(LevelManager.Instance.savedBoss[k], transform.position + new Vector3(-3, 0, 0), Quaternion.identity);

                newBoss.GetComponent<Boss>().bossNumber = k + 1;
            }
        }
    }

    IEnumerator LaunchRoom()
    {
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
    }
}
