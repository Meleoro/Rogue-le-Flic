using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        activeGun = Instantiate(ManagerChara.Instance.activeGun, ManagerChara.Instance.transform.position, Quaternion.identity, transform);

        if(ManagerChara.Instance.stockWeapon != null)
            stockedGun = Instantiate(ManagerChara.Instance.stockWeapon, ManagerChara.Instance.transform.position, Quaternion.identity, transform);

        ManagerChara.Instance.activeGun = activeGun;
        ManagerChara.Instance.activeGun.GetComponent<Gun>().isHeld = true;

        if (stockedGun != null)
        {
            ManagerChara.Instance.stockWeapon = stockedGun;
            ManagerChara.Instance.stockWeapon.GetComponent<Gun>().isStocked = true;
        }

        currentLevel += 1;

        if(currentLevel == 1)
        {
            SceneManager.LoadScene(level1);
        }

        else if (currentLevel == 2)
        {
            SceneManager.LoadScene(level2);
        }

        else
        {
            SceneManager.LoadScene(level3);
        }

    }
}
