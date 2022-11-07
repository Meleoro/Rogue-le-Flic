using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float delaySpawn;
    [SerializeField] private float health;
    private float timer;

    [SerializeField] private GameObject beaver;
    [SerializeField] private bool spawnBeavers;
    
    [SerializeField] private GameObject frog;
    [SerializeField] private bool spawnFrogs;

    private List<GameObject> ennemies = new List<GameObject>();
    
    
    void Start()
    {
        timer = delaySpawn / 2;
        
        if(spawnBeavers)
            ennemies.Add(beaver);
        
        if(spawnFrogs)
            ennemies.Add(frog);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = delaySpawn;
            SpawnEntity();
        }
    }

    
    void SpawnEntity()
    {
        int ennemySpawning = Random.Range(0, ennemies.Count);
        
        Vector3 modificateurPos = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
        
        Instantiate(ennemies[ennemySpawning],  new Vector3(transform.position.x + 1 * Mathf.Sign(modificateurPos.x), 
            transform.position.y + 1.2f * Mathf.Sign(modificateurPos.y), 0), Quaternion.identity);
    }
}
