using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using DG.Tweening;
using Image = UnityEngine.UI.Image;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Levels")]
    [SerializeField] private string tuto;
    [SerializeField] private string level1;
    [SerializeField] private string level2;
    [SerializeField] private string level3;
    [SerializeField] [Range(0, 3)] private int startLevel;
    [HideInInspector] public int currentLevel;

    [Header("Save Choices")]
    public List<GameObject> savedBoss = new List<GameObject>();
    public List<int> banishedRooms;
    
    [Header("HurtBosses")] 
    public GameObject beaverHurt;
    public GameObject frogBoss;
    public GameObject turtleBoss;

    [Header("Save Weapons")]
    public GameObject activeGun;
    public GameObject stockedGun;
    
    [Header("Save Module")]
    public GameObject module1;
    public GameObject module2;



    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        else
            Destroy(gameObject);
    }


    void Start()
    {
        currentLevel = startLevel;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.N))
        {
            ChangeScene();
        }
    }

    public void ChangeScene()
    {
        if (!ManagerChara.Instance.activeGun.GetComponent<Gun>().isDontDestoy)
        {
            activeGun = Instantiate(ManagerChara.Instance.activeGun, ManagerChara.Instance.transform.position, Quaternion.identity, transform);

            Destroy(ManagerChara.Instance.activeGun);

            ManagerChara.Instance.activeGun = activeGun;
            ManagerChara.Instance.activeGun.GetComponent<Gun>().isHeld = true;
            ManagerChara.Instance.activeGun.GetComponent<Gun>().isDontDestoy = true;
        }


        if(ManagerChara.Instance.stockWeapon != null)
        {
            if (!ManagerChara.Instance.stockWeapon.GetComponent<Gun>().isDontDestoy)
            {
                stockedGun = Instantiate(ManagerChara.Instance.stockWeapon, ManagerChara.Instance.transform.position, Quaternion.identity, transform);

                Destroy(ManagerChara.Instance.stockWeapon);

                ManagerChara.Instance.stockWeapon = stockedGun;
                ManagerChara.Instance.stockWeapon.GetComponent<Gun>().isStocked = true;
                ManagerChara.Instance.stockWeapon.GetComponent<Gun>().isDontDestoy = true;
            }
        }

        ManagerChara.Instance.reload.GetComponentInParent<Canvas>().enabled = false;
        DashChara.Instance.dashEffects.weight = 0;
        
        
        currentLevel += 1;

        if(currentLevel == 1)
        {
            StartCoroutine(FonduManager.Instance.ChangeScene(level1, false));
        }

        else if (currentLevel == 2)
        {
            StartCoroutine(FonduManager.Instance.ChangeScene(level2, false));
        }

        else
        {
            StartCoroutine(FonduManager.Instance.ChangeScene(level3, false));
        }
    }
}
