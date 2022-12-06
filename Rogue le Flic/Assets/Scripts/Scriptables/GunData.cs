using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "GunData")]
public class GunData : ScriptableObject
{
    [Header("ShotProprieties")] 
    public int damages;
    public int nbrBulletPerShot;
    public float cooldownShot;
    public float shotDispersion;
    public float charaKnockback;
    public AnimationCurve gunKnockback;

    [Header("TirChargeable")] 
    public bool tirChargeable;
    public float dureeChargement;

    [Header("Munitions")] 
    public int maxAmmo;
    
    [Header("Reload")] 
    public int chargeurSize;
    public float reloadTime;

    [Header("CameraShake")] 
    public float shakeDuration;
    public float shakeAmplitude;

    [Header("Light")] 
    public float lightShotDuration;
    public float lightShotIntensity;

    //[Header("Modules")] 
    
}

