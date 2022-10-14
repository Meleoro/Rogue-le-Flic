using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class KickChara : MonoBehaviour
{
    public static KickChara Instance;

    public GameObject kick;
    
    public float kickDuration;
    public float kickStrenght;

    public float propulsionChara;

    [Header("Effets")] 
    public float cameraShakeDuration;
    public float cameraShakeAmplitude;
    [Range(0.1f, 5)] public float slowMoSpeed;
    [Range(1, 50)] public float slowMoStrenght;
    private bool slowMoActive;
    private float timerSlowMo;


    public void Awake()
    {
        Instance = this;

        kick.SetActive(false);
    }

    private void Update()
    {
        if (slowMoActive)
        {
            timerSlowMo += Time.fixedDeltaTime * slowMoSpeed;
            
            Time.timeScale = Mathf.Lerp(1 / slowMoStrenght, 1, timerSlowMo);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            
            if (timerSlowMo >= 1)
            {
                slowMoActive = false;
            }
        }
    }


    public IEnumerator Kick()
    {
        // ON RECUPERE LA POSITION DE LA SOURIS ET DU JOUEUR 
        Vector2 mousePos = ReferenceCamera.Instance.camera.ScreenToWorldPoint(ManagerChara.Instance.controls.Character.MousePosition.ReadValue<Vector2>());
        Vector2 charaPos = ManagerChara.Instance.transform.position;
        
        // ON PLACE LA ZONE DE KICK ET ON L'AFFICHE
        kick.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg, Vector3.forward);
        kick.SetActive(true);
        
        // ON PROPULSE LE PERSONNAGE ET ON RETIRE LE CONTROLE QUE LE JOUEUR A SUR LUI
        ManagerChara.Instance.noControl = true;
        ManagerChara.Instance.rb.AddForce(new Vector2(mousePos.x - charaPos.x, mousePos.y - charaPos.y).normalized * propulsionChara, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(kickDuration);
        
        // ON ENLEVE LA ZONE DE KICK ET REDONNE LE CONTROLE AU JOUEUR
        kick.SetActive(false);
        ManagerChara.Instance.noControl = false;
    }

    public void CameraShake()
    {
        ReferenceCamera.Instance.transform.DOShakePosition(cameraShakeDuration, cameraShakeAmplitude);
    }

    public void SlowMo()
    {
        Time.timeScale = 1 / slowMoStrenght;

        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        timerSlowMo = 0;
        slowMoActive = true;
    }
}
