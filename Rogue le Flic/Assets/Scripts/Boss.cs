using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class Boss : MonoBehaviour
{
    public enum boss
    {
        Beaver,
        Frog,
        Turtle
    }

    public boss bossType;

    public bool isHurt;
    
    [Header("Feedbacks")] 
    public Color hitColor;

    [HideInInspector] public int bossNumber;

    private BeaverBoss beaverScript;
    private FrogBoss frogScript;
    private TurtleBoss turtleScript;

    private bool death;
    private bool canShake;
    private bool lookLeft;
    private bool doOnce;
    private Vector3 originalScale;

    [HideInInspector] public bool canMove;


    [Header("References")]
    public Animator anim;
    public GameObject sprite;
    public GameObject spawnIndicator;
    public BoxCollider2D _collider2D;
    

    private void Start()
    {
        switch (bossType)
        {
            case boss.Beaver:
                beaverScript = GetComponent<BeaverBoss>();

                if (isHurt)
                {
                    beaverScript.currentHealth = beaverScript.bossData.health / 2;
                }
                else
                {
                    beaverScript.currentHealth = beaverScript.bossData.health;
                }

                break;

            
            case boss.Frog:
                frogScript = GetComponent<FrogBoss>();
                
                if (isHurt)
                {
                    frogScript.currentHealth = frogScript.bossData.health / 2;
                }
                else
                {
                    frogScript.currentHealth = frogScript.bossData.health;
                }
                
                break;

            
            case boss.Turtle:
                turtleScript = GetComponent<TurtleBoss>();
                
                if (isHurt)
                {
                    turtleScript.currentHealth = turtleScript.bossData.health / 2;
                }
                else
                {
                    turtleScript.currentHealth = turtleScript.bossData.health;
                }

                break;
        }

        canShake = false;
    }
    

    private void Update()
    {
        if (!death && canMove)
        {
            switch (bossType)
            {
                case boss.Beaver:
                    beaverScript.BeaverBehavior();
                    break;

                case boss.Frog:
                    frogScript.FrogBehavior();
                    break;

                case boss.Turtle:
                    turtleScript.TurtleBehavior();
                    break;
            }
        }
        else if(canShake)
        {
            canShake = false;
            transform.DOShakePosition(0.12f, 0.2f).OnComplete(() => canShake = true);
        }

        if (ReferenceChoice.Instance.kicked || ReferenceChoice.Instance.spared)
        {
            if (!doOnce)
            {
                doOnce = true;
                
                if (ReferenceChoice.Instance.spared)
                {
                    switch (bossType)
                    {
                        case boss.Beaver:
                            LevelManager.Instance.savedBoss.Add(LevelManager.Instance.beaverHurt);
                            break;

                        case boss.Frog:
                            LevelManager.Instance.savedBoss.Add(LevelManager.Instance.frogBoss);
                            break;

                        case boss.Turtle:
                            LevelManager.Instance.savedBoss.Add(LevelManager.Instance.turtleBoss);
                            break;
                    }
                
                }
            
                StartCoroutine(EndCinematicDeath());
            }
        }
    }

    private void FixedUpdate()
    {
        if (!death && canMove)
        {
            switch (bossType)
            {
                case boss.Beaver:
                    beaverScript.FixedBeaverBehavior();
                    break;

                case boss.Frog:
                    frogScript.FixedFrogBehavior();
                    break;

                case boss.Turtle:
                    turtleScript.FixedTurtleBehavior();
                    break;
            }
        }
    }


    public void TakeDamages(int damages, GameObject bullet)
    {
        SpriteRenderer[] sprites = sprite.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer k in sprites)
        {
            StartCoroutine(FeedbackDamage(k));
        }
        
        switch (bossType)
        { 
            case boss.Beaver:
                beaverScript.TakeDamages(damages, bullet);
                break;

            case boss.Frog:
                frogScript.TakeDamages(damages, bullet);
                break;

            case boss.Turtle:
                turtleScript.TakeDamages(damages, bullet);
                break;
        }
    }
    
    public IEnumerator FeedbackDamage(SpriteRenderer currentSprite)
    {
        currentSprite.DOColor(hitColor, 0.12f);

        yield return new WaitForSeconds(0.12f);
        
        currentSprite.DOColor(Color.white, 0.12f);
    }

    public void Death()
    {
        StopChoroutines();
        
        if (!death && !isHurt)
        {
            death = true;
            StartCoroutine(CinematicDeath());
        }
        else if(isHurt)
        {
            Destroy(gameObject);
        }
    }


    public void VerifyBossType()
    {
        switch (bossType)
        {
            case boss.Beaver:
                ReferenceCamera.Instance.bossSprite.sprite = ReferenceCamera.Instance.beaver;
                break;

            
            case boss.Frog:
                ReferenceCamera.Instance.bossSprite.sprite = ReferenceCamera.Instance.frog;
                break;

            
            case boss.Turtle:
                ReferenceCamera.Instance.bossSprite.sprite = ReferenceCamera.Instance.turtle;
                break;
        }
    }


    public void StopChoroutines()
    {
        switch (bossType)
        {
            case boss.Beaver:
                //beaverScript.StopCoroutines();
                break;

            case boss.Frog:
                frogScript.StopChoroutines();
                break;

            case boss.Turtle:
                break;
        }

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }
    
    
    IEnumerator CinematicDeath()
    {
        transform.localScale = Vector3.one;
        
        switch (bossType)
        {
            case boss.Beaver:
                foreach (GameObject k in beaverScript.ennemies)
                {
                    Destroy(k);
                }
                break;

            case boss.Frog:
                foreach (GameObject k in frogScript.ennemies)
                {
                    Destroy(k);
                }
                break;

            case boss.Turtle:
                foreach (GameObject k in turtleScript.ennemies)
                {
                    Destroy(k);
                }
                break;
        }
        
        CameraMovements.Instance.Reboot();
        
        anim.SetBool("isWalking", false);
        
        ManagerChara.Instance.savePosition = ManagerChara.Instance.transform.position;

        ReferenceCamera.Instance.fondNoir.DOFade(1, 2);
        ReferenceCamera.Instance.finalCinematic = true;

        ManagerChara.Instance.noControl = true;

        CameraMovements.Instance.bossEndRoom = true;
        CameraMovements.Instance.timeZoom = 2;

        switch (bossType)
        {
            case boss.Beaver:
                if (!beaverScript.lookLeft)
                {
                    CameraMovements.Instance.posCamera = transform.position + new Vector3(4.5f, 0, 0);
                    lookLeft = false;
                }
                else
                {
                    CameraMovements.Instance.posCamera = transform.position + new Vector3(-4.5f, 0, 0);
                    lookLeft = true;
                }
                break;

            case boss.Frog:
                if (!frogScript.lookLeft)
                {
                    CameraMovements.Instance.posCamera = transform.position + new Vector3(4.5f, 0, 0);
                    lookLeft = false;
                }
                else
                {
                    CameraMovements.Instance.posCamera = transform.position + new Vector3(-4.5f, 0, 0);
                    lookLeft = true;
                }
                break;

            case boss.Turtle:
                if (!turtleScript.lookLeft)
                {
                    CameraMovements.Instance.posCamera = transform.position + new Vector3(4.5f, 0, 0);
                    lookLeft = false;
                }
                else
                {
                    CameraMovements.Instance.posCamera = transform.position + new Vector3(-4.5f, 0, 0);
                    lookLeft = true;
                }
                break;
        }

        yield return new WaitForSeconds(2);
        
        ManagerChara.Instance.StopChoroutines();
        ManagerChara.Instance.noControl = true;


        transform.position = transform.position + new Vector3(0, -5000, 0);
        CameraMovements.Instance.transform.position = CameraMovements.Instance.transform.position + new Vector3(0, -5000, 0);

        ReferenceCamera.Instance.finalCinematicChara = true;

        if (lookLeft)
        {
            ManagerChara.Instance.transform.position = transform.position + new Vector3(-20, 0, 0);

            ManagerChara.Instance.transform.DOMoveX(transform.position.x - 10, 2).SetEase(Ease.Linear);
        }

        else
        {
            ManagerChara.Instance.transform.position = transform.position + new Vector3(20, 0, 0);

            ManagerChara.Instance.transform.DOMoveX(transform.position.x + 10, 2).SetEase(Ease.Linear);
        }
        
        ManagerChara.Instance.anim.SetBool("isWalking", true);

        canShake = true;

        yield return new WaitForSeconds(2);

        
        ManagerChara.Instance.anim.SetBool("isWalking", false);

        ReferenceChoice.Instance.kick.DOLocalMoveX(400, 2);
        ReferenceChoice.Instance.spare.DOLocalMoveX(-400, 2);

        yield return new WaitForSeconds(2);


        ReferenceChoice.Instance.kick.GetComponent<Animator>().enabled = true;
        ReferenceChoice.Instance.spare.GetComponent<Animator>().enabled = true;
    }
    

    public IEnumerator EndCinematicDeath()
    {
        ReferenceChoice.Instance.kick.GetComponent<Animator>().enabled = false;
        ReferenceChoice.Instance.spare.GetComponent<Animator>().enabled = false;
        
        ReferenceChoice.Instance.kick.DOLocalMoveX(800, 1);
        ReferenceChoice.Instance.spare.DOLocalMoveX(-800, 1);
        
        
        // ON CHOISI DE KICK
        if (ReferenceChoice.Instance.kicked)
        {
            ManagerChara.Instance.anim.SetBool("isWalking", true);
            
            if (lookLeft)
            {
                ManagerChara.Instance.transform.DOMoveX(transform.position.x - 5, 1).SetEase(Ease.Linear);
            }
            else
            {
                ManagerChara.Instance.transform.DOMoveX(transform.position.x + 5, 1).SetEase(Ease.Linear);
            }
            
            yield return new WaitForSeconds(1f);
            
            ManagerChara.Instance.anim.SetBool("isWalking", false);
            
            yield return new WaitForSeconds(0.2f);
            
            ManagerChara.Instance.anim.SetTrigger("isKicking");
            
            if (ManagerChara.Instance.transform.position.x > transform.position.x)
            {
                StartCoroutine(KickChara.Instance.Kick(Vector2.right));
            }
            else
            {
                StartCoroutine(KickChara.Instance.Kick(Vector2.left));
            }
            
            yield return new WaitForSeconds(0.1f);
            
            anim.SetTrigger("death");
            
            yield return new WaitForSeconds(1f);
        }

        // ON CHOISI DE SPARE
        else
        {
            canShake = false;
            
            StartCoroutine(JumpChoroutine());

            yield return new WaitForSeconds(1f);
        }


        Vector2 offsetCamera = ManagerChara.Instance.transform.position - CameraMovements.Instance.transform.position;

        ManagerChara.Instance.transform.position = ManagerChara.Instance.savePosition;
        CameraMovements.Instance.transform.position = ManagerChara.Instance.savePosition + offsetCamera;
        
        CameraMovements.Instance.posCamera = ManagerChara.Instance.transform.position;
        CameraMovements.Instance.timeZoom = 0.2f;
        CameraMovements.Instance.Reboot();

        ReferenceCamera.Instance.fondNoir.DOFade(0, 1);

        yield return new WaitForSeconds(1);


        switch (bossType)
        {
            case boss.Beaver:
                LevelManager.Instance.banishedRooms.Add(0);
                break;

            case boss.Frog:
                LevelManager.Instance.banishedRooms.Add(1);
                break;

            case boss.Turtle:
                LevelManager.Instance.banishedRooms.Add(2);
                break;
        }
        
        ReferenceCamera.Instance.finalCinematic = false;
        ReferenceCamera.Instance.finalCinematicChara = false;

        ManagerChara.Instance.noControl = false;
        CameraMovements.Instance.bossEndRoom = false;

        MapManager.Instance.activeRoom.GetComponent<DoorManager>().PortesActives();

        Destroy(gameObject);
    }
    
    
    IEnumerator JumpChoroutine()
    {
        StartCoroutine(Decollage(0.4f, 0.1f));

        yield return new WaitForSeconds(0.4f);

        sprite.transform.DOMoveY(transform.position.y + 30, 0.2f).SetEase(Ease.InCirc);
    }

    IEnumerator Decollage(float duration1, float duration2)
    {
        originalScale = sprite.transform.localScale;
        
        sprite.transform.DOScale(new Vector3(originalScale.x + 0.2f, originalScale.y - 0.2f, originalScale.z), duration1);
        
        yield return new WaitForSeconds(duration1 / 2);
        
        //anim.SetTrigger("isJumping");
        
        yield return new WaitForSeconds(duration1 / 2);
        
        sprite.transform.DOScale(new Vector3(originalScale.x - 0.1f, originalScale.y + 0.1f, originalScale.z), duration2);
    }
}