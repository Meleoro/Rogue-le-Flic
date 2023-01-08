using System.Collections;
using System.Collections.Generic;
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
    private int currentLevel;

    [Header("Save Choices")]
    public List<GameObject> savedBoss = new List<GameObject>();
    public List<int> banishedRooms;


    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
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
