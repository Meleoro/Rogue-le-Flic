using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthChara : MonoBehaviour
{

    public int maxhealth;
    public int health;
    public int damage;
    public static HealthChara instance;
    public GameObject player;
    
    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance d'Interact Counter dans la scène");
            return;
        }
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        health = maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(damage);
        }
    }
    
    public void TakeDamage(int dmg)
    {
        
        health = health - dmg;
        
        if (health <= 0)
        {
            Destroy(player);
        }
    }

}
