using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "GunData")]
public class GunData : ScriptableObject
{
    [Header("Shop")] 
    public int itemPrice;
    public string itemName;
    public string itemDesciption;
    
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
    public Sprite charge1;
    public Sprite charge2;
    public Sprite charge3;
    public Sprite charge4;
    public Sprite charge0;
    
    
    [Header("Reload")] 
    public int chargeurSize;
    public float reloadTime;

    [Header("CameraShake")] 
    public float shakeDuration;
    public float shakeAmplitude;

    [Header("Light")] 
    public float lightShotDuration;
    public float lightShotIntensity;


    [Header("Son")] public AudioSource shoot;
    

    //[Header("Modules")] 

}

