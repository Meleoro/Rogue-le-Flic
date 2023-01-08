using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pathfinding;
using UnityEngine.UI;

public class TurtleBoss : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float speedX;
    public float speedY;

    [Header("Deceleration")]
    public float dragDeceleration;
    public float dragMultiplier;

    [Header("Turtle")]
    [SerializeField] private int health;
    private int currentHealth;
    [SerializeField] private float cooldownMin;
    [SerializeField] private float cooldownMax;
    [SerializeField] private float stunDuration;
    private float timer;
    private bool isAttacking;
    private bool canMove;
    private int currentAttack;
    private Vector2 direction;
    [HideInInspector] public bool lookLeft;
    private float stunTimer;
    [HideInInspector] public bool isKicked;

    [Header("Charge Basique")]
    [SerializeField] private float chargeVitesseOriginale;
    [SerializeField] private float gainVitesseRebond;
    [SerializeField] private float vitesseMax;
    [HideInInspector] public bool isSliding;
    private Vector2 directionSlide;
    private float currentSpeed;

    [Header("Charge Puissante")]
    [SerializeField] private float chargemementDuree;
    [SerializeField] private float chargePuissanteStrenght;
    [SerializeField] private int minBoxSpawn;
    [SerializeField] private int maxBoxSpawn;
    [SerializeField] private GameObject box;

    [Header("Spawn")]
    [SerializeField] private int minTurtleSpawn;
    [SerializeField] private int maxTurtleSpawn;
    [SerializeField] private GameObject turtle;
    private int cooldownSpawn;

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

        boss.anim.SetBool("isWalking", false);
    }


    public void TurtleBehavior()
    {
        if (stunTimer <= 0)
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
                        currentAttack = Random.Range(1, 3);
                    }
                    else
                    {
                        currentAttack = 1;
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
        }

        else
        {
            stunTimer -= Time.deltaTime;
            //boss.anim.SetTrigger("reset");
        }
    }


    public void FixedTurtleBehavior()
    {
        if (!isAttacking && stunTimer <= 0)
        {
            direction = AIPath.targetDirection;

            rb.AddForce(new Vector2(direction.x * speedX, direction.y * speedY) * 5, ForceMode2D.Force);

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
        currentSpeed = chargeVitesseOriginale;
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Ennemy") && isSliding)
        {
            if(currentSpeed < vitesseMax)
                currentSpeed += gainVitesseRebond;

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
    }


    public void Kicked(Vector2 direction)
    {
        directionSlide = direction;
        isKicked = true;
    }



    IEnumerator GigeCharge()
    {
        yield return new WaitForSeconds(0.75f);
    }


    public void Stun()
    {
        stunTimer = stunDuration;

        StopAllCoroutines();

        isAttacking = false;
        timer = Random.Range(cooldownMin, cooldownMax);

        rb.AddForce(directionSlide.normalized * 25, ForceMode2D.Impulse);

        boss.anim.SetTrigger("reset");

        CollideWall();
    }


    IEnumerator Spawn()
    {
        transform.DOShakePosition(1, 1);

        yield return new WaitForSeconds(1);

        int nbrEnnemies = Random.Range(minTurtleSpawn, maxTurtleSpawn + 1);

        List<int> indexSelected = new List<int>();

        for (int i = 0; i < nbrEnnemies; i++)
        {
            int newIndex = Random.Range(0, bossRoom.spawnPoints.Count);

            while (indexSelected.Contains(newIndex))
            {
                newIndex = Random.Range(0, bossRoom.spawnPoints.Count);
            }

            Instantiate(turtle, bossRoom.spawnPoints[newIndex].position, Quaternion.identity);
            indexSelected.Add(newIndex);
        }

        isAttacking = false;
        timer = Random.Range(cooldownMin, cooldownMax);

        cooldownSpawn = 2;
    }


    public void TakeDamages(int degats, GameObject bullet)
    {
        currentHealth -= degats;

        healthBar.fillAmount = (float)currentHealth / health;

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
        int nbrItems = Random.Range(minBoxSpawn, maxBoxSpawn + 1);

        List<int> indexSelected = new List<int>();

        for (int i = 0; i < nbrItems; i++)
        {
            int newIndex = Random.Range(0, bossRoom.spawnPoints.Count);

            while (indexSelected.Contains(newIndex))
            {
                newIndex = Random.Range(0, bossRoom.spawnPoints.Count);
            }

            GameObject newBox = Instantiate(box, bossRoom.spawnPoints[newIndex].position, Quaternion.identity);

            StartCoroutine(newBox.GetComponent<Box>().Fall());
            indexSelected.Add(newIndex);
        }

        stunTimer = stunDuration;
        ReferenceCamera.Instance._camera.DOShakePosition(1, 1f);
    }
}
