using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "FrogBossData")]
public class FrogBossData : ScriptableObject
{
    [Header("Movement Speeds")]
    public float speedX;
    public float speedY;

    [Header("Deceleration")]
    public float dragDeceleration;
    public float dragMultiplier;

    [Header("Frog")]
    public int health;
    public float cooldownMin;
    public float cooldownMax;
    public Vector2 posLeft;
    public Vector2 posRight;
    public float stunDuration;

    [Header("Saut")]
    public float distanceJumpTrigger;

    [Header("Attaque sautée")]
    public float duree;
    public int minBoxSpawn;
    public int maxBoxSpawn;
    public GameObject box;

    [Header("Spawn")]
    public int minFrogSpawn;
    public int maxFrogSpawn;
    public GameObject frog;

    [Header("Tir")]
    public AnimationCurve tonguePatern;
    public GameObject tongue;
    public float shotDuration;
}
