using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pathfinding;
using UnityEngine.UI;

public class FrogBoss : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float speedX;
    public float speedY;

    [Header("Deceleration")]
    public float dragDeceleration;
    public float dragMultiplier;

    [Header("Frog")]
    [SerializeField] private int health;
    private int currentHealth;
    [SerializeField] private float cooldownMin;
    [SerializeField] private float cooldownMax;
    [SerializeField] private Vector2 posLeft;
    [SerializeField] private Vector2 posRight;
    private float timer;
    private bool isAttacking;
    private bool canMove;
    private int currentAttack;
    private Vector2 direction;
    private bool lookLeft;

    [Header("Saut")]
    [SerializeField] private float distanceJumpTrigger;
    [SerializeField] private List<Transform> spotsJump;
    private Vector2 jumpDestination;
    
    [Header("Attaque sautée")]
    [SerializeField] private float duree;
    [SerializeField] private int minBoxSpawn;
    [SerializeField] private int maxBoxSpawn;
    [SerializeField] private float stunDuration;
    [SerializeField] private GameObject box;
    private float stunTimer;

    [Header("Spawn")]
    [SerializeField] private int minFrogSpawn;
    [SerializeField] private int maxFrogSpawn;
    [SerializeField] private GameObject frog;
    private int cooldownSpawn;

    [Header("Tir")]
    [SerializeField] private AnimationCurve TonguePatern;

    [Header("References")]
    public AIPath AIPath;
    public AIDestinationSetter AIDestination;
    [SerializeField] private BossRoom bossRoom;
    [SerializeField] private Image healthBar;
    private Rigidbody2D rb;
    private Boss boss;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = dragDeceleration * dragMultiplier;

        canMove = true;

        AIDestination.target = ManagerChara.Instance.transform;

        boss = GetComponent<Boss>();

        timer = Random.Range(cooldownMin, cooldownMax);
        currentAttack = 0;
        currentHealth = health;
    }


    public void FrogBehavior()
    {
        if(stunTimer <= 0)
        {
            // COOLDOWN ENTRE LES ATTAQUES
            if (!isAttacking)
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    isAttacking = true;

                    if (cooldownSpawn > 0)
                    {
                        currentAttack = Random.Range(1, 5);
                    }
                    else
                    {
                        currentAttack = Random.Range(1, 6);
                    }
                }
            }

            // ATTAQUE
            if (isAttacking && currentAttack != 0)
            {
                isAttacking = true;
                
                float distance = Mathf.Sqrt(Mathf.Pow(AIPath.destination.x - transform.position.x, 2) + 
                                            Mathf.Pow(AIPath.destination.y - transform.position.y, 2));

                if (distance < distanceJumpTrigger)
                {
                    Jump();
                }
                
                /*else
                {
                    boss.anim.SetTrigger("isAttacking");

                    cooldownSpawn -= 1;

                    // ATTAQUE SAUTEE
                    if (currentAttack == 1)
                    {
                    
                    }

                    // SPAWN
                    else if (currentAttack == 5)
                    {
                        StartCoroutine(Spawn());
                    }

                    // TIR
                    else
                    {
                    
                    }

                    currentAttack = 0;
                }*/


                currentAttack = 0;
            }

            // ROTATION
            if (direction.x > 0.1f)
            {
                lookLeft = false;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        
            else if (direction.x < -0.1f)
            {
                lookLeft = true;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        else
        {
            stunTimer -= Time.deltaTime;
        }

        if (!isAttacking)
        {
            if (lookLeft)
            {
                boss.sprite.transform.localPosition = posLeft;
            }
            else
            {
                boss.sprite.transform.localPosition = posRight;
            }
        }
    }
    

    public void FixedFrogBehavior()
    {
        if (!isAttacking && stunTimer <= 0)
        {
            direction = AIPath.targetDirection;

            rb.AddForce(new Vector2(direction.x * speedX, direction.y * speedY) * 5, ForceMode2D.Force);

            boss.anim.SetBool("isWalking", true);
        }

        else
        {
            boss.anim.SetBool("isWalking", false);
        }
    }



    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Vector2 directionProj = col.transform.position - transform.position;

            HealthManager.Instance.LoseHealth(directionProj);
        }

        /*else if (isKicked && col.CompareTag("Ennemy"))
        {
            col.GetComponent<Ennemy>().TakeDamages(2, gameObject);
        }

        else if (isKicked)
        {
            TakeDamages(2, gameObject);
        }*/
    }


    private void Jump()
    {
        float maxDistance = 0;

        for (int k = 0; k < spotsJump.Count; k++)
        {
            float newDistance = Vector2.Distance(ManagerChara.Instance.transform.position, spotsJump[k].position);

            if(newDistance > maxDistance)
            {
                jumpDestination = spotsJump[k].position;
                maxDistance = newDistance;
            }
        }


        StartCoroutine(JumpChoroutine(jumpDestination));
    }

    
    IEnumerator JumpChoroutine(Vector2 destination)
    {
        boss.spawnIndicator.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = false;
        boss._collider2D.enabled = false;
        
        boss.sprite.transform.DOMoveY(transform.position.y + 15, 0.2f).SetEase(Ease.InCirc);
        
        boss.spawnIndicator.transform.DOScale(new Vector3(2f, 2f, 2f), 0);
        boss.spawnIndicator.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 2);

        transform.DOMove(destination, 4);
        
        yield return new WaitForSeconds(0.2f);

        boss.sprite.SetActive(false);
        
        yield return new WaitForSeconds(1.8f);
        
        boss.spawnIndicator.transform.DOScale(new Vector3(2f, 2f, 2f), 2);

        yield return new WaitForSeconds(1.8f);
        
        boss.sprite.SetActive(true);
        
        boss.sprite.transform.DOMoveY(transform.position.y, 0.2f).SetEase(Ease.InCirc);;
        
        yield return new WaitForSeconds(0.2f);
        
        boss.spawnIndicator.SetActive(false);
        
        GetComponent<BoxCollider2D>().enabled = true;
        boss._collider2D.enabled = true;
        
        isAttacking = false;
        timer = Random.Range(cooldownMin, cooldownMax);
    }
    
    
    IEnumerator Spawn()
    {
        transform.DOShakePosition(1, 1);

        yield return new WaitForSeconds(1);

        int nbrEnnemies = Random.Range(minFrogSpawn, maxFrogSpawn + 1);

        List<int> indexSelected = new List<int>();

        for (int i = 0; i < nbrEnnemies; i++)
        {
            int newIndex = Random.Range(0, bossRoom.spawnPoints.Count);

            while (indexSelected.Contains(newIndex))
            {
                newIndex = Random.Range(0, bossRoom.spawnPoints.Count);
            }

            Instantiate(frog, bossRoom.spawnPoints[newIndex].position, Quaternion.identity);
            indexSelected.Add(newIndex);
        }

        isAttacking = false;
        timer = Random.Range(cooldownMin, cooldownMax);

        cooldownSpawn = 2;
    }


    public void TakeDamages(int degats, GameObject bullet)
    {
        currentHealth -= degats;

        healthBar.fillAmount = (float) currentHealth / health;

        if (currentHealth <= 0)
        {
            boss.Death();
        }
    }
}
