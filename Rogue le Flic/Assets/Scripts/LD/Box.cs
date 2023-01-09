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

    public int damageFromBox;

    private float originalY;
    private bool isFalling;

    [HideInInspector] public bool isInvincible;


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
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player"))
        {
            if(!isKicked && !isInvincible)
                Explose();

            else if(col.gameObject.CompareTag("Ennemy") && !isInvincible)
            {
                col.gameObject.GetComponent<Ennemy>().TakeDamages(damageFromBox, gameObject);
                Explose();
            }

            else if (col.gameObject.CompareTag("Boss"))
            {
                if (isInvincible)
                {
                    col.gameObject.GetComponent<Boss>().TakeDamages(damageFromBox, gameObject);
                    col.gameObject.GetComponent<FrogBoss>().Stun();
                    Explose();
                }
                else
                {
                    col.gameObject.GetComponent<Boss>().TakeDamages(damageFromBox, gameObject);
                    Explose();
                }
            }

            else if(!col.gameObject.CompareTag("Box") && !isInvincible)
            {
                Explose();
            }
        }
    }

    public void Explose()
    {
        for (int k = 0; k < nbrFragments; k++)
        {
            Vector3 basePos = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.1f, 0.5f));
            
            GameObject currentFragment = Instantiate(fragment, transform.position + basePos, Quaternion.identity);

            if(Random.Range(0, 2) == 0)
                currentFragment.GetComponent<Fragment>().goLeft = true;
        }
        
        Destroy(gameObject);
    }

    public void Kicked(Vector2 direction)
    {
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
