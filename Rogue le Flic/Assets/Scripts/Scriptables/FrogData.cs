using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "FrogData")]
public class FrogData : ScriptableObject
{
    [Header("Movement Speeds")] 
    public float speedX;
    public float speedY;

    [Header("Deceleration")] 
    public float dragDeceleration;
    public float dragMultiplier;
    
    [Header("Frog")]
    public int health;
    public GameObject tongue;
    public float distanceShotTrigger;
    public float shotDuration;
    public AnimationCurve tonguePatern;
}
