using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;


public class Box : MonoBehaviour
{
    [SerializeField] private GameObject fragment;
    [SerializeField] private int nbrFragments;

    [SerializeField] private GameObject shadow;

    private bool isKicked;
    private Vector2 directionKick;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private Animator anim;

    [SerializeField] private List<Sprite> piecesSprites;

    private float originalY;
    private bool isFalling;

    [HideInInspector] public bool isInvincible;


    public AudioSource audiosource1;
    


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }


    private void Update()
    {
        if (!isKicked
            && !isFalling && !isInvincible)
        {
            rb.velocity = Vector2.zero;
            
            if (ManagerChara.Instance.isDashing)
            {
                boxCollider2D.enabled = false;
            }
            else 
            { 
                boxCollider2D.enabled = true;
            }
        }
        else if(isKicked)
        {
            boxCollider2D.enabled = false;
        }
        
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player"))
        {
            if(!isKicked && !isInvincible)
                Explose();

            else if(col.gameObject.CompareTag("Ennemy") && !isInvincible)
            {
                col.gameObject.GetComponent<Ennemy>().TakeDamages(DegatsManager.Instance.degatsBox, gameObject);
                col.gameObject.GetComponent<Ennemy>().Stun();
                Explose();
            }

            else if (col.gameObject.CompareTag("Boss"))
            {
                if (isInvincible)
                {
                    col.gameObject.GetComponent<Boss>().TakeDamages(DegatsManager.Instance.degatsBox, gameObject);
                    col.gameObject.GetComponent<FrogBoss>().Stun();
                    Explose();
                }
                else
                {
                    col.gameObject.GetComponent<Boss>().TakeDamages(DegatsManager.Instance.degatsBox, gameObject);
                    Explose();
                }
            }
            
            else if (col.gameObject.CompareTag("Trou"))
            {
                
            }

            else if(!col.gameObject.CompareTag("Box") && !isInvincible)
            {
                Explose();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (isKicked)
        {
            if (!col.CompareTag("Player") && !col.CompareTag("Kick"))
            {
                if(!isKicked && !isInvincible)
                    Explose();

                else if(col.gameObject.CompareTag("Ennemy") && !isInvincible)
                {
                    col.gameObject.GetComponent<Ennemy>().TakeDamages(DegatsManager.Instance.degatsBox, gameObject);
                    col.gameObject.GetComponent<Ennemy>().Stun();
                    Explose();
                }

                else if (col.gameObject.CompareTag("Boss"))
                {
                    if (isInvincible)
                    {
                        col.gameObject.GetComponent<Boss>().TakeDamages(DegatsManager.Instance.degatsBox, gameObject);
                        col.gameObject.GetComponent<FrogBoss>().Stun();
                        Explose();
                    }
                    else
                    {
                        col.gameObject.GetComponent<Boss>().TakeDamages(DegatsManager.Instance.degatsBox, gameObject);
                        Explose();
                    }
                }
            

                else if(!col.CompareTag("Box") && !col.CompareTag("Trou") && !col.CompareTag("Gun") && !isInvincible)
                {
                    Explose();
                }
            }
        }
    }


    public void CollisionWithTrou()
    {
        boxCollider2D.enabled = false;
    }
    

    public void Explose()
    {
        for (int k = 0; k < nbrFragments; k++)
        {
            Vector3 basePos = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.1f, 0.5f));
            
            GameObject currentFragment = Instantiate(fragment, transform.position + basePos, Quaternion.identity);

            currentFragment.GetComponent<SpriteRenderer>().sprite = piecesSprites[Random.Range(0, piecesSprites.Count)];

            if(Random.Range(0, 2) == 0)
                currentFragment.GetComponent<Fragment>().goLeft = true;
        }
        
        Destroy(gameObject);
    }

    public void Kicked(Vector2 direction)
    {
        
        if (!audiosource1.isPlaying)
        {
            audiosource1.Play();
        }

        
        boxCollider2D.enabled = false;
        
        isKicked = true;
        directionKick = direction;
        
        rb.AddForce(directionKick * 25, ForceMode2D.Impulse);
    }

    
    public IEnumerator Fall()
    {
        anim = GetComponent<Animator>();
        anim.enabled = true;
        
        originalY = transform.position.y;
        boxCollider2D = GetComponent<BoxCollider2D>();
        
        boxCollider2D.enabled = false;
        
        transform.DOMoveY(originalY + 10, 0);

        isFalling = true;

        transform.DOMoveY(originalY, 0.3f).SetEase(Ease.InCirc);

        yield return new WaitForSeconds(0.3f);
        
        anim.enabled = false;
        isFalling = false;
        
        boxCollider2D.enabled = true;
    }
}
