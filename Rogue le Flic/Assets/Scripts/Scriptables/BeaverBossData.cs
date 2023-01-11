using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "BeaverBossData")]
public class BeaverBossData : ScriptableObject
{
    [Header("Movement Speeds")]
    public float speedX;
    public float speedY;

    [Header("Deceleration")]
    public float dragDeceleration;
    public float dragMultiplier;

    [Header("Castor")] 
    public int health;
    public float cooldownMin;
    public float cooldownMax;

    [Header("Charge")]
    public float strenghtJump;

    [Header("Spawn")]
    public int minCastorSpawn;
    public int maxCastorSpawn;
    public GameObject castor;

    [Header("GigaCharge")]
    public float strenghtGigaJump;
    public int minBoxSpawn;
    public int maxBoxSpawn;
    public float stunDuration;
    public GameObject box;
}
