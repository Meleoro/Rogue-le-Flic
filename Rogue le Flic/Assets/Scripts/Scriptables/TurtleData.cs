using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "TurtleData")]
public class TurtleData : ScriptableObject
{
    [Header("Movement Speeds")]
    public float speedX;
    public float speedY;

    [Header("Deceleration")]
    public float dragDeceleration;
    public float dragMultiplier;

    [Header("Turtle")]
    public int health;
    public float distanceSlideTrigger;
    public float speedSlide;
    public int nbrRebonds;
    public float cooldown;
}
