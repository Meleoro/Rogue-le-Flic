using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "TurtleBossData")]
public class TurtleBossData : ScriptableObject
{
    [Header("Movement Speeds")]
    public float speedX;
    public float speedY;

    [Header("Deceleration")]
    public float dragDeceleration;
    public float dragMultiplier;

    [Header("Turtle")] 
    public int health;
    public float cooldownMin;
    public float cooldownMax;
    public float stunDuration;

    [Header("Charge Basique")]
    public float chargeVitesseOriginale;
    public float gainVitesseRebond;
    public float vitesseMax;
    public int rebondsMax;
    
    [Header("Charge Puissante")]
    public float chargemementDuree;
    public float chargePuissanteSpeed;
    public int minBoxSpawn;
    public int maxBoxSpawn;
    public GameObject box;

    [Header("Spawn")]
    public int minTurtleSpawn;
    public int maxTurtleSpawn;
    public GameObject turtle;
}
