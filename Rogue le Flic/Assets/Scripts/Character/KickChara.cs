using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class KickChara : MonoBehaviour
{
    public static KickChara Instance;

    public GameObject kick;
    
    public float kickDuration;
    public float kickStrenght;
    [SerializeField] private float kickDelay;
    
    private float originalZoom;
    [HideInInspector] public GameObject kickedEnnemy;

    public float propulsionChara;
    [HideInInspector] public Vector2 kickDirection;

    public bool mouseDirection;

    [Header("Effets Kick")] 
    public float cameraShakeDuration;
    public float cameraShakeAmplitude;
    [Range(0.1f, 10)] public float slowMoSpeed;
    [Range(1, 50)] public float slowMoStrenght;
    [Range(0.1f, 4)] public float newZoom;
    [Range(0.001f, 0.3f)] public float zoomMoment;
    [Range(0.001f, 0.3f)] public float dezoomMoment;
    private bool slowMoActive;
    private bool slowMoStrongActive;
    private float timerSlowMo;
    private float currentMultiplier;
    private bool hasKicked;
    
    /*
    [Header("Effets Coup Critique")] 
    public float cameraShakeDurationCritical;
    public float cameraShakeAmplitudeCritical;
    [Range(0.1f, 5)] public float slowMoSpeedCritical;
    [Range(1, 50)] public float slowMoStrenghtCritical;
    [Range(0.1f, 4)] public float newZoomCritical;
    [Range(0.001f, 0.3f)] public float zoomMomentCritical;
    [Range(0.001f, 0.3f)] public float dezoomMomentCritical;*/

    /*[Header("Auto-Aim")] 
    private bool autoAimActive;*/

    [Header("References")] 
    public Volume kickVolume;
    [SerializeField] private Animation foot;


    public void Awake()
    {
        Instance = this;

        kick.SetActive(false);
    }


    private void FixedUpdate()
    {
        if (slowMoActive)
        {
            timerSlowMo += Time.fixedDeltaTime * slowMoSpeed * currentMultiplier;
        }
    }


    private void Update()
    {
        // INTERRUPTION D'UN ADVERSAIRE
        /*if (slowMoStrongActive)
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
        }*/

        if (slowMoActive)
        {
            // Gestion du déroulement du temps
            Time.timeScale = Mathf.Lerp(1 / slowMoStrenght, 1, timerSlowMo);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            
            // Effets visuels
            kickVolume.weight = Mathf.Lerp(0.70f / currentMultiplier, 0, timerSlowMo);

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


    public IEnumerator Kick(Vector2 direction)
    {
        // ON RECUPERE LA POSITION DE LA SOURIS ET DU JOUEUR 
        /*Vector2 mousePos = ReferenceCamera.Instance._camera.ScreenToWorldPoint(ManagerChara.Instance.controls.Character.MousePosition.ReadValue<Vector2>());
        Vector2 charaPos = ManagerChara.Instance.transform.position;*/

        hasKicked = false;

        /*if (mouseDirection)
        {
            kickDirection = new Vector2(-mousePos.x + charaPos.x, -mousePos.y + charaPos.y).normalized;
        }
        else
        {
            kickDirection = -MovementsChara.Instance.direction.normalized;
        }*/

        kickDirection = direction;

        kick.SetActive(true);
        kick.GetComponent<CircleCollider2D>().enabled = false;
        foot.DORestart();
        
        ManagerChara.Instance.noControl = true;

        if (direction.x < 0)
        {
            ManagerChara.Instance.anim.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            
            MovementsChara.Instance.sprite.transform.localPosition = ManagerChara.Instance.posRight;
        }
        else
        {
            ManagerChara.Instance.anim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            
            MovementsChara.Instance.sprite.transform.localPosition = ManagerChara.Instance.posLeft;
        }

        yield return new WaitForSeconds(kickDelay);

        // ON PLACE LA ZONE DE KICK ET ON L'AFFICHE
        kick.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(-kickDirection.y, -kickDirection.x) * Mathf.Rad2Deg, Vector3.forward);
        kick.GetComponent<CircleCollider2D>().enabled = true;

        // ON PROPULSE LE PERSONNAGE
        ManagerChara.Instance.rb.AddForce(new Vector2(-kickDirection.x, -kickDirection.y).normalized * propulsionChara, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(kickDuration);
        
        // ON ENLEVE LA ZONE DE KICK ET REDONNE LE CONTROLE AU JOUEUR
        kick.SetActive(false);
        ManagerChara.Instance.noControl = false;
        ManagerChara.Instance.activeGun.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void CameraShake()
    {
        ReferenceCamera.Instance.transform.DOShakePosition(cameraShakeDuration, cameraShakeAmplitude);
    }

    /*public void SlowMoStrong()
    {
        if (!slowMoStrongActive)
        {
            Time.timeScale = 1 / slowMoStrenghtCritical;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            originalZoom = ReferenceCamera.Instance._camera.orthographicSize;

            timerSlowMo = 0;
            slowMoStrongActive = true;
        }
    }*/

    public void SlowMo(float multiplier)
    {
        if (!slowMoActive && !hasKicked)
        {
            hasKicked = true;

            Time.timeScale = 1 / slowMoStrenght;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            originalZoom = ReferenceCamera.Instance._camera.orthographicSize;

            timerSlowMo = 0;
            slowMoActive = true;

            currentMultiplier = multiplier;
        }
    }

    /*public void AutoAim()
    {
        if(ManagerChara.Instance.activeGun != null)
            StartCoroutine(ManagerChara.Instance.activeGun.GetComponent<Gun>().AutoAim(kickDuration));
    }*/
}
