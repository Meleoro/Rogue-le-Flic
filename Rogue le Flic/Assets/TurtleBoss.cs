using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pathfinding;
using UnityEngine.UI;

public class TurtleBoss : MonoBehaviour
{
    public TurtleBossData bossData;

    [Header("Turtle")]
    [HideInInspector] public int currentHealth;
    private float timer;
    private bool isAttacking;
    private bool canMove;
    private int currentAttack;
    private Vector2 direction;
    [HideInInspector] public bool lookLeft;
    private float stunTimer;
    [HideInInspector] public bool isKicked;

    [Header("Charge Basique")]
    [HideInInspector] public bool isSliding;
    private Vector2 directionSlide;
    private float currentSpeed;

    [Header("Charge Puissante")]
    private bool isGigaSliding;
    private float timerShake;
    private bool canShake;

    [Header("Spawn")]
    private int cooldownSpawn;

    [Header("References")]
    public AIPath AIPath;
    public AIDestinationSetter AIDestination;
    [SerializeField] private BossRoom bossRoom;
    public Image healthBar;
    [SerializeField] private GameObject VFXStun;
    private Rigidbody2D rb;
    private Boss boss;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = bossData.dragDeceleration * bossData.dragMultiplier;

        canMove = true;

        AIDestination.target = ManagerChara.Instance.transform;

        boss = GetComponent<Boss>();

        timer = Random.Range(bossData.cooldownMin, bossData.cooldownMax);
        currentAttack = 0;

        boss.anim.SetBool("isWalking", false);
    }


    public void TurtleBehavior()
    {
        if (stunTimer <= 0)
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

                    if (cooldownSpawn > 0)
                    {
                        currentAttack = Random.Range(1, 3);
                    }
                    else
                    {
                        currentAttack = 2;
                    }
                }
            }

            // ATTAQUE
            if (isAttacking && currentAttack != 0)
            {
                isAttacking = true;
                cooldownSpawn -= 1;

                // ATTAQUE SAUTEE
                if (currentAttack == 1)
                {
                    StartCoroutine(Charge());
                }

                // SPAWN
                else if (currentAttack == 3 && cooldownSpawn <= 0)
                {
                    StartCoroutine(Spawn());
                }

                // TIR
                else
                {
                    StartCoroutine(GigeCharge());
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

            if (timerShake > 0)
            {
                timerShake -= Time.deltaTime;

                if (canShake)
                {
                    canShake = false;
                    transform.DOShakePosition(0.05f, (bossData.chargemementDuree - timerShake) / 2).OnComplete((() => canShake = true));
                }
            }
        }

        else
        {
            stunTimer -= Time.deltaTime;
            
            VFXStun.SetActive(true);
            //boss.anim.SetTrigger("reset");
        }
    }


    public void FixedTurtleBehavior()
    {
        direction = AIPath.targetDirection;
        
        if (!isAttacking && stunTimer <= 0)
        {
            direction = AIPath.targetDirection;

            rb.AddForce(new Vector2(direction.x * bossData.speedX, direction.y * bossData.speedY) * 5, ForceMode2D.Force);

            boss.anim.SetBool("isWalking", true);
        }

        else if (isSliding)
        {
            boss.anim.SetBool("isWalking", false);

            if (!isKicked)
                rb.AddForce(directionSlide * currentSpeed, ForceMode2D.Force);

            else
                rb.AddForce(directionSlide * currentSpeed * 1.5f, ForceMode2D.Force);
        }
        
        else if (isGigaSliding)
        {
            if (!isKicked)
                rb.AddForce(directionSlide * bossData.chargePuissanteSpeed, ForceMode2D.Force);

            else
                rb.AddForce(directionSlide * bossData.chargePuissanteSpeed * 1.5f, ForceMode2D.Force);
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

        else if (col.gameObject.CompareTag("Ennemy") && isSliding)
        {
            if (!isKicked)
                col.gameObject.GetComponent<Ennemy>().TakeDamages(2, gameObject);

            else
                col.gameObject.GetComponent<Ennemy>().TakeDamages(20, gameObject);
        }

        else if (col.gameObject.CompareTag("Box") && isSliding)
        {
            col.gameObject.GetComponent<Box>().Explose();
        }

        else if (isKicked && col.CompareTag("Ennemy"))
        {
            col.GetComponent<Ennemy>().TakeDamages(20, gameObject);
        }

        else if (isKicked)
        {
            TakeDamages(5, gameObject);
        }
    }


    IEnumerator Charge()
    {
        canMove = false;

        boss.anim.SetTrigger("StartAttack");
        boss.anim.SetBool("isWalking", false);

        transform.DOShakePosition(0.75f, 0.3f);

        directionSlide = -direction.normalized;

        yield return new WaitForSeconds(0.75f);

        isSliding = true;
        currentSpeed = bossData.chargeVitesseOriginale;
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Ennemy") && isSliding)
        {
            if(currentSpeed < bossData.vitesseMax)
                currentSpeed += bossData.gainVitesseRebond;

            directionSlide = Vector3.Reflect(directionSlide.normalized, col.contacts[0].normal);

            if (isKicked)
            {
                TakeDamages(10, col.gameObject);

                ReferenceCamera.Instance.transform.DOShakePosition(1, 1);

                Stun();

                isKicked = false;
                isSliding = false;
            }
        }
        
        else if (!col.gameObject.CompareTag("Ennemy") && isGigaSliding)
        {
            TakeDamages(10, col.gameObject);

            ReferenceCamera.Instance.transform.DOShakePosition(1, 1);

            Stun();

            isKicked = false;
            isGigaSliding = false;
        }
    }


    public void Kicked(Vector2 direction)
    {
        directionSlide = direction;
        isKicked = true;
    }



    IEnumerator GigeCharge()
    {
        boss.anim.SetTrigger("StartAttack");
        boss.anim.SetBool("isWalking", false);

        canShake = true;
        timerShake = bossData.chargemementDuree;
        
        yield return new WaitForSeconds(bossData.chargemementDuree);
        
        canMove = false;

        directionSlide = (ManagerChara.Instance.transform.position - transform.position).normalized;

        isGigaSliding = true;
        currentSpeed = bossData.chargeVitesseOriginale;
    }


    public void Stun()
    {
        stunTimer = bossData.stunDuration;

        StopAllCoroutines();

        isAttacking = false;
        timer = Random.Range(bossData.cooldownMin, bossData.cooldownMax);

        rb.AddForce(directionSlide.normalized * 25, ForceMode2D.Impulse);

        boss.anim.SetTrigger("reset");

        CollideWall();
    }


    IEnumerator Spawn()
    {
        transform.DOShakePosition(1, 1);

        yield return new WaitForSeconds(1);

        int nbrEnnemies = Random.Range(bossData.minTurtleSpawn, bossData.maxTurtleSpawn + 1);

        List<int> indexSelected = new List<int>();

        for (int i = 0; i < nbrEnnemies; i++)
        {
            int newIndex = Random.Range(0, bossRoom.spawnPoints.Count);

            while (indexSelected.Contains(newIndex))
            {
                newIndex = Random.Range(0, bossRoom.spawnPoints.Count);
            }

            Instantiate(bossData.turtle, bossRoom.spawnPoints[newIndex].position, Quaternion.identity);
            indexSelected.Add(newIndex);
        }

        isAttacking = false;
        timer = Random.Range(bossData.cooldownMin, bossData.cooldownMax);

        cooldownSpawn = 2;
    }


    public void TakeDamages(int degats, GameObject bullet)
    {
        currentHealth -= degats;

        healthBar.fillAmount = (float)currentHealth / bossData.health;

        if (bullet.CompareTag("Box"))
        {
            rb.AddForce(-direction.normalized * 20, ForceMode2D.Impulse);
        }

        if (currentHealth <= 0)
        {
            boss.Death();
        }
    }


    public void CollideWall()
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
