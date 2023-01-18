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
    private bool bounceWall;
    private float sens;

    [Header("Modules")]
    [HideInInspector] public bool percante;
    [HideInInspector] public int nbrPercesMax;
    private int currentPerces;
    [HideInInspector] public bool rebondissante;
    [HideInInspector] public int nbrRebondsMax;
    private int currentRebonds;

    [Header("Autres")]
    [SerializeField] private BoxCollider2D bounceWalls;
    public GameObject objetAGrossir;
    [HideInInspector] public Vector2 directionBullet;
    private Rigidbody2D rb;

    private bool isPercing;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        timerBubble = timeExplosion;

        direction = transform.right;

        currentPerces = 0;
        currentRebonds = 0;
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
            if (!stopMoving && !bounceWall)
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
                        transform.GetComponent<SpriteRenderer>().DOFade(0.4f, 0.3f);
                        doOnce = true;
                    }

                    if(bulletSpeed > 0)
                        bulletSpeed -= Time.deltaTime * arrowDeceleration * 2;
                }
                
                else
                {
                    rb.velocity = direction * bulletSpeed;

                    bulletSpeed -= Time.deltaTime * arrowDeceleration;
                }
            }

            if (bounceWall)
            {
                rb.velocity = direction * bulletSpeed;

                timerArrow -= Time.deltaTime;

                if(sens < 0)
                    transform.Rotate(0, 0, 0.1f * bulletSpeed);

                else
                    transform.Rotate(0, 0, -0.1f * bulletSpeed);

                if (!doOnce)
                {
                    transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 1);
                    transform.GetComponent<SpriteRenderer>().DOFade(0.4f, 1f);
                    doOnce = true;
                }

                if (bulletSpeed > 0)
                    bulletSpeed -= Time.deltaTime * arrowDeceleration;

                if (timerArrow <= 0)
                {
                    rb.velocity = Vector2.zero;
                    Destroy(this);
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
            bulletDamages = (int) (bulletDamages * ModuleManager.Instance.multiplicateurDegatsCrit);
            bulletSpeed *= ModuleManager.Instance.multiplicateurVitesseCrit;
            objetAGrossir.transform.localScale *= ModuleManager.Instance.multiplicateurTailleCrit;
            
            isCritique = false;
        }
    }

    private void LateUpdate()
    {
        directionBullet = rb.velocity;
    }


    IEnumerator isPercing2()
    {
        isPercing = true;
        
        yield return new WaitForSeconds(0.04f);

        isPercing = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ennemy"))
        {
            if (!isPercing)
            {
                collision.GetComponent<Ennemy>().TakeDamages(bulletDamages, gameObject);

                if (!percante || currentPerces >= nbrPercesMax)
                    Destroy(gameObject);
            
                else
                    currentPerces += 1;

                bounceWalls.enabled = false;

                StartCoroutine(isPercing2());
            }
        }

        else if (collision.CompareTag("Boss"))
        {
            collision.GetComponent<Boss>().TakeDamages(bulletDamages, gameObject);

            if (!percante || currentPerces >= nbrPercesMax)
                Destroy(gameObject);

            else
                currentPerces += 1;

            bounceWalls.enabled = false;
        }

        else if (collision.CompareTag("Box"))
        {
            if (!percante || currentPerces >= nbrPercesMax)
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
            
            else
            {
                currentPerces += 1;
                collision.GetComponent<Box>().Explose();
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
            if (!rebondissante || nbrRebondsMax <= currentRebonds)
            {
                if (isArrow)
                {
                    bounceWalls.enabled = true;
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            else
                bounceWalls.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isArrow && nbrRebondsMax <= currentRebonds)
        {
            bounceWall = true;

            bulletSpeed = bulletSpeed / 2;
            sens = UnityEngine.Random.Range(-1f, 1f);
            timerArrow = 1f;
        }
        
        if (collision.gameObject.CompareTag("Box"))
        {
            collision.gameObject.GetComponent<Box>().Explose();
        }
        
        direction = Vector3.Reflect(direction.normalized, collision.contacts[0].normal);


        if (!isArrow)
        {
            transform.rotation = Quaternion.Euler(0, 0, Angle(direction) - 90);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, Angle(direction) + 90);
        }


        bounceWalls.enabled = false;

        currentRebonds += 1;
    }
    
    
    public static float Angle(Vector2 vector2)
    {
        return 360 - (Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg);
    }
}

