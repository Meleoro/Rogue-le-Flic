using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;
using DG.Tweening;

public class BeaverBoss : MonoBehaviour
{
    [Header("Levels")] 
    public BeaverBossData niveau1;
    public BeaverBossData niveau2;
    public BeaverBossData niveau3;
    public BeaverBossData affaibli;
    public BeaverBossData alone;

    [HideInInspector] public BeaverBossData bossData;

    [Header("Castor")]
    [HideInInspector] public int currentHealth;
    [HideInInspector] public bool lookLeft;
    private float timer;
    private bool isAttacking;
    private bool canMove;
    private int currentAttack;
    private Vector2 direction;

    [Header("Charge")]
    private bool isCharging;

    [Header("Spawn")]
    private int cooldownSpawn;

    [Header("GigaCharge")]
    private bool isGigaCharging;
    private float stunTimer;

    [Header("References")]
    public AIPath AIPath;
    public AIDestinationSetter AIDestination;
    [HideInInspector] public BossRoom bossRoom;
    [HideInInspector] public Image healthBar;
    [SerializeField] private GameObject VFXStun;
    private Rigidbody2D rb;
    private Boss boss;

    public List<GameObject> ennemies = new List<GameObject>();


    public AudioSource stun;
    public AudioSource windUp;
    
    
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = bossData.dragDeceleration * bossData.dragMultiplier;

        canMove = true;

        AIDestination.target = ManagerChara.Instance.transform;

        timer = Random.Range(bossData.cooldownMin, bossData.cooldownMax);
        currentAttack = 0;
    }


    public void InitialiseBoss()
    {
        if (LevelManager.Instance.currentLevel == 1)
        {
            bossData = niveau1;
        }
        else if (LevelManager.Instance.currentLevel == 2)
        {
            bossData = niveau2;
        }
        else if (LevelManager.Instance.currentLevel == 3)
        {
            bossData = niveau3;
        }
        
        boss = GetComponent<Boss>();

        if (boss.isHurt)
        {
            bossData = affaibli;
        }

        else if (boss.isAlone)
        {
            bossData = alone;
        }
        
        
        if (boss.bossNumber == 0)
        {
            ReferenceBossUI.Instance.object1.SetActive(true);
            healthBar = ReferenceBossUI.Instance.healthBar1;
        }
        else if (boss.bossNumber == 1)
        {
            ReferenceBossUI.Instance.object2.SetActive(true);
            healthBar = ReferenceBossUI.Instance.healthBar2;
        }
        else if (boss.bossNumber == 2)
        {
            ReferenceBossUI.Instance.object3.SetActive(true);
            healthBar = ReferenceBossUI.Instance.healthBar3;
        }
        
        healthBar.fillAmount = (float) currentHealth / bossData.health;
    }


    public void BeaverBehavior()
    {
        if(stunTimer <= 0)
        {
            VFXStun.SetActive(false);
            healthBar.fillAmount = (float) currentHealth / bossData.health;

            // COOLDOWN ENTRE LES ATTAQUES
            if (!isAttacking)
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    isAttacking = true;

                    if (cooldownSpawn > 0 || boss.isHurt)
                    {
                        currentAttack = Random.Range(1, 3);
                    }
                    else
                    {
                        currentAttack = Random.Range(3, 4);
                    }
                }
            }

            // ATTAQUE
            if (isAttacking && currentAttack != 0)
            {
                cooldownSpawn -= 1;

                // CHARGE
                if (currentAttack == 1)
                {
                    boss.anim.SetTrigger("isAttacking");
                    
                    StartCoroutine(Charge(new Vector2(AIPath.destination.x - transform.position.x, AIPath.destination.y - transform.position.y)));
                }

                // SPAWN
                else if (currentAttack == 3)
                {
                    boss.anim.SetTrigger("isSummoning");
                    
                    StartCoroutine(Spawn());
                }

                // GIGA CHARGE
                else
                {
                    boss.anim.SetTrigger("isAttacking");
                    
                    StartCoroutine(GigaCharge(new Vector2(AIPath.destination.x - transform.position.x, AIPath.destination.y - transform.position.y)));
                }

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
            
            VFXStun.SetActive(true);
            
            //stun.Play();
        }
    }
    

    public void FixedBeaverBehavior()
    {
        if (!isAttacking && stunTimer <= 0)
        {
            direction = AIPath.targetDirection;

            rb.AddForce(new Vector2(direction.x * bossData.speedX, direction.y * bossData.speedY) * 5, ForceMode2D.Force);

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

        else if(isGigaCharging && CompareTag("Wall"))
        {
            CollideWall();
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



    IEnumerator Charge(Vector2 directionJump)
    {
        rb.AddForce(-directionJump.normalized * (bossData.strenghtJump / 5), ForceMode2D.Impulse);

        //windUp.Play();
        
        
        yield return new WaitForSeconds(0.6f);

        isCharging = true;

        rb.AddForce(directionJump.normalized * bossData.strenghtJump, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.25f);

        isAttacking = false;
        isCharging = false;
        timer = Random.Range(bossData.cooldownMin, bossData.cooldownMax);
    }


    IEnumerator Spawn()
    {
        transform.DOShakePosition(1, 0.5f);

        yield return new WaitForSeconds(1);

        int nbrEnnemies = Random.Range(bossData.minCastorSpawn, bossData.maxCastorSpawn + 1);

        List<int> indexSelected = new List<int>();

        for (int i = 0; i < nbrEnnemies; i++)
        {
            int newIndex = Random.Range(0, bossRoom.spawnPoints.Count);

            while (indexSelected.Contains(newIndex))
            {
                newIndex = Random.Range(0, bossRoom.spawnPoints.Count);
            }

            ennemies.Add(Instantiate(bossData.castor, bossRoom.spawnPoints[newIndex].position, Quaternion.identity));
            indexSelected.Add(newIndex);
        }

        isAttacking = false;
        timer = Random.Range(bossData.cooldownMin, bossData.cooldownMax);

        cooldownSpawn = 2;
    }


    IEnumerator GigaCharge(Vector2 directionJump)
    {
        rb.AddForce(-directionJump.normalized * (bossData.strenghtGigaJump / 5), ForceMode2D.Impulse);

        //windUp.Play();
        
        
        yield return new WaitForSeconds(1f);

        isGigaCharging = true;

        rb.AddForce(directionJump.normalized * bossData.strenghtGigaJump, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.32f);

        isAttacking = false;
        isGigaCharging = false;
        timer = Random.Range(bossData.cooldownMin, bossData.cooldownMax);
    }



    public void TakeDamages(int degats, GameObject bullet)
    {
        currentHealth -= degats;

        healthBar.fillAmount = (float) currentHealth / bossData.health;

        if (currentHealth <= 0)
        {
            boss.Death();
        }
    }

    public void CollideWall()
    {
        if (isGigaCharging || isCharging)
        {
            int nbrItems = Random.Range(bossData.minBoxSpawn, bossData.maxBoxSpawn + 1);

            List<int> indexSelected = new List<int>();

            for (int i = 0; i < nbrItems; i++)
            {
                int newIndex = Random.Range(0, bossRoom.spawnPoints.Count);

                while (indexSelected.Contains(newIndex))
                {
                    newIndex = Random.Range(0, bossRoom.spawnPoints.Count);
                }

                GameObject newBox = Instantiate(bossData.box, bossRoom.spawnPoints[newIndex].position, Quaternion.identity);

                StartCoroutine(newBox.GetComponent<Box>().Fall());
                indexSelected.Add(newIndex);
            }

            stunTimer = bossData.stunDuration;
            ReferenceCamera.Instance._camera.DOShakePosition(1, 1f);
        }
    }
}
