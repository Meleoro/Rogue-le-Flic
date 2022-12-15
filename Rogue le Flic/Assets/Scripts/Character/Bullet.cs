using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [Header("General")]
    public float bulletSpeed;
    public float bulletKnockback;
    public int bulletDamages;
    private Vector2 direction;
    [HideInInspector] public bool isCritique;

    [Header("Bubble")] 
    public bool isBubble;
    public float decelerationBubble;
    public float timeExplosion;
    [SerializeField] private GameObject bubble;
    [SerializeField] private GameObject bubbleExplosion;
    private float timerBubble;
    [HideInInspector] public Vector2 originalVelocity;

    [Header("Arrow")] 
    public bool isArrow;
    [SerializeField] private float arrowDeceleration;
    public float arrowDuration1;
    public float arrowDuration2;
    public float arrowDuration3;
    public float arrowDuration4;
    [HideInInspector] public float timerArrow;
    private bool stopMoving;
    private bool doOnce;

    [Header("Modules")]
    [HideInInspector] public bool percante;
    [HideInInspector] public bool rebondissante;

    [Header("Autres")]
    [SerializeField] private BoxCollider2D bounceWalls;
    public GameObject objetAGrossir;
    [HideInInspector] public Vector2 directionBullet;
    private Rigidbody2D rb;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        timerBubble = timeExplosion;

        direction = transform.right;
    }

    void Update()
    {
        // BULLE
        if (isBubble)
        {
            rb.velocity = originalVelocity / 13.5f * bulletSpeed + direction * bulletSpeed;

            timerBubble -= Time.deltaTime;
            
            if (timerBubble <= 0)
            {
                bubble.SetActive(false);
                bubbleExplosion.SetActive(true);
            }
            else
            {
                bulletSpeed -= Time.deltaTime * decelerationBubble;
            }

            if (timerBubble <= -0.5f)
            {
                Destroy(gameObject);
            }
        }
        
        // ARROW
        else if (isArrow)
        {
            if (!stopMoving)
            {
                rb.velocity = direction * bulletSpeed;

                timerArrow -= Time.deltaTime;

                if (timerArrow <= 0)
                {
                    stopMoving = true;
                    rb.velocity = Vector2.zero;
                    GetComponent<CircleCollider2D>().enabled = false;
                    GetComponent<BoxCollider2D>().enabled = false;
                }
                
                else if (timerArrow <= 0.3f)
                {
                    if (!doOnce)
                    {
                        transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f);
                        doOnce = true;
                    }

                    if(bulletSpeed > 0)
                        bulletSpeed -= Time.deltaTime * arrowDeceleration * 2;
                }
                
                else
                {
                    bulletSpeed -= Time.deltaTime * arrowDeceleration;
                }
            }
        }

        else
        {
            rb.velocity = direction * bulletSpeed;
        }

        // SI LE TIRE EST CRITIQUE
        if (isCritique)
        {
            bulletDamages = (int) (bulletDamages * ModuleManager.Instance.multiplicateurDegats);
            bulletSpeed *= ModuleManager.Instance.multiplicateurVitesse;
            objetAGrossir.transform.localScale *= ModuleManager.Instance.multiplicateurTaille;
            
            isCritique = false;
        }
    }

    private void LateUpdate()
    {
        directionBullet = rb.velocity;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ennemy"))
        {
            collision.GetComponent<Ennemy>().TakeDamages(bulletDamages, gameObject);

            if (!percante)
                Destroy(gameObject);

            bounceWalls.enabled = false;
        }
        
        else if (collision.CompareTag("Box"))
        {
            if (!rebondissante)
            {
                Destroy(gameObject);
                collision.GetComponent<Box>().Explose();
            }

            else
            {
                bounceWalls.enabled = true;
            }
        }

        else if (collision.CompareTag("ExplosiveBox"))
        {
            if (!rebondissante)
            {
                Destroy(gameObject);
                collision.GetComponent<ExplosiveBox>().Explose();
            }

            else
            {
                bounceWalls.enabled = true;
            }
        }
        
        else if (collision.CompareTag("Trou"))
        {
            bounceWalls.enabled = false;
        }

        else
        {
            if (!rebondissante)
                Destroy(gameObject);

            else
                bounceWalls.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            collision.gameObject.GetComponent<Box>().Explose();
        }
        
        direction = Vector3.Reflect(direction.normalized, collision.contacts[0].normal);

        bounceWalls.enabled = false;
    }
}

