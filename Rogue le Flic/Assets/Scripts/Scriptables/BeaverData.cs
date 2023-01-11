using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "BeaverData")]
public class BeaverData : ScriptableObject
{
    [Header("Movement Speeds")] 
    public float speedX;
    public float speedY;

    [Header("Deceleration")] 
    public float dragDeceleration;
    public float dragMultiplier;
    
    [Header("Castor")]
    public int health;
    public float distanceJumpTrigger;
    public float strenghtJump;
}
