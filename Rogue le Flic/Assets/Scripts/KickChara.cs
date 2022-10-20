using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class KickChara : MonoBehaviour
{
    public static KickChara Instance;

    public GameObject kick;
    
    public float kickDuration;
    public float kickStrenght;

    public float propulsionChara;

    [Header("Effets Kick Normal")] 
    public float cameraShakeDuration;
    public float cameraShakeAmplitude;
    [Range(0.1f, 5)] public float slowMoSpeed;
    [Range(1, 50)] public float slowMoStrenght;
    [Range(0.1f, 4)] public float newZoom;
    [Range(0.001f, 0.3f)] public float zoomMoment;
    [Range(0.001f, 0.3f)] public float dezoomMoment;
    private bool slowMoActive;
    private bool slowMoStrongActive;
    private float timerSlowMo;
    
    [Header("Effets Coup Critique")] 
    public float cameraShakeDurationCritical;
    public float cameraShakeAmplitudeCritical;
    [Range(0.1f, 5)] public float slowMoSpeedCritical;
    [Range(1, 50)] public float slowMoStrenghtCritical;
    [Range(0.1f, 4)] public float newZoomCritical;
    [Range(0.001f, 0.3f)] public float zoomMomentCritical;
    [Range(0.001f, 0.3f)] public float dezoomMomentCritical;
    
    
    private float originalZoom;

    [Header("Auto-Aim")] 
    private bool autoAimActive;
    [HideInInspector] public GameObject kickedEnnemy;

    [Header("References")] 
    public Volume kickVolume;


    public void Awake()
    {
        Instance = this;

        kick.SetActive(false);
    }

    private void Update()
    {
        // INTERRUPTION D'UN ADVERSAIRE
        if (slowMoStrongActive)
        {
            timerSlowMo += Time.fixedDeltaTime * slowMoSpeedCritical;

            // Gestion du déroulement du temps
            Time.timeScale = Mathf.Lerp(1 / slowMoStrenghtCritical, 1, timerSlowMo);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            
            // Effets visuels
            kickVolume.weight = Mathf.Lerp(1, 0, timerSlowMo);

            // Mouvements camera
            if (timerSlowMo < zoomMomentCritical)
            {
                ReferenceCamera.Instance._camera.orthographicSize = Mathf.Lerp(originalZoom, originalZoom - newZoomCritical, timerSlowMo / zoomMomentCritical);
            }
            else if (timerSlowMo >= zoomMomentCritical + dezoomMomentCritical)
            {
                ReferenceCamera.Instance._camera.orthographicSize = Mathf.Lerp(originalZoom - newZoomCritical, originalZoom, timerSlowMo / (1 - dezoomMomentCritical + zoomMomentCritical));
            }
            
            // Auto-Aim
            kickedEnnemy.GetComponent<Ennemy>().cible.SetActive(true);

            if (timerSlowMo >= 1)
            {
                slowMoStrongActive = false;
                
                kickedEnnemy.GetComponent<Ennemy>().cible.SetActive(false);
            }
        }
        
        // KICK NORMAL
        else if (slowMoActive)
        {
            timerSlowMo += Time.fixedDeltaTime * slowMoSpeed;

            // Gestion du déroulement du temps
            Time.timeScale = Mathf.Lerp(1 / slowMoStrenght, 1, timerSlowMo);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            
            // Effets visuels
            kickVolume.weight = Mathf.Lerp(0.70f, 0, timerSlowMo);

            // Mouvements camera
            if (timerSlowMo < zoomMoment)
            {
                ReferenceCamera.Instance._camera.orthographicSize = Mathf.Lerp(originalZoom, originalZoom - newZoom, timerSlowMo / zoomMoment);
            }
            else if (timerSlowMo >= zoomMoment + dezoomMoment)
            {
                ReferenceCamera.Instance._camera.orthographicSize = Mathf.Lerp(originalZoom - newZoom, originalZoom, timerSlowMo / (1 - dezoomMoment + zoomMoment));
            }

            if (timerSlowMo >= 1)
            {
                slowMoActive = false;
            }
        }
    }


    public IEnumerator Kick()
    {
        // ON RECUPERE LA POSITION DE LA SOURIS ET DU JOUEUR 
        Vector2 mousePos = ReferenceCamera.Instance._camera.ScreenToWorldPoint(ManagerChara.Instance.controls.Character.MousePosition.ReadValue<Vector2>());
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

    public void SlowMoStrong()
    {
        if (!slowMoStrongActive)
        {
            Time.timeScale = 1 / slowMoStrenghtCritical;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            originalZoom = ReferenceCamera.Instance._camera.orthographicSize;

            timerSlowMo = 0;
            slowMoStrongActive = true;
        }
    }

    public void SlowMo()
    {
        if (!slowMoActive)
        {
            Time.timeScale = 1 / slowMoStrenght;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            originalZoom = ReferenceCamera.Instance._camera.orthographicSize;

            timerSlowMo = 0;
            slowMoActive = true;
        }
    }

    public void AutoAim()
    {
        if(ManagerChara.Instance.activeGun != null)
            StartCoroutine(ManagerChara.Instance.activeGun.GetComponent<Gun>().AutoAim(kickDuration));
    }
}
