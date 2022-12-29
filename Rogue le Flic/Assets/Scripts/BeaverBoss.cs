using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;
using DG.Tweening;

public class BeaverBoss : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float speedX;
    public float speedY;

    [Header("Deceleration")]
    public float dragDeceleration;
    public float dragMultiplier;

    [Header("Castor")]
    [SerializeField] private int health;
    private int currentHealth;
    [SerializeField] private float cooldownMin;
    [SerializeField] private float cooldownMax;
    private float timer;
    private bool isAttacking;
    private bool canMove;
    private int currentAttack;
    private Vector2 direction;

    [Header("Charge")]
    [SerializeField] private float distanceJumpTrigger;
    [SerializeField] private float strenghtJump;
    private bool isCharging;

    [Header("Spawn")]
    [SerializeField] private int minCastorSpawn;
    [SerializeField] private int maxCastorSpawn;
    [SerializeField] private GameObject castor;

    [Header("GigaCharge")]
    [SerializeField] private float strenghtGigaJump;
    private bool isGigaCharging;

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


    public void BeaverBehavior()
    {
        if (!isAttacking)
        {
            timer -= Time.deltaTime;
            
            if (timer <= 0)
            {
                isAttacking = true;

                currentAttack = Random.Range(1, 4);
            }
        }

        if (isAttacking && currentAttack != 0)
        {
            // CHARGE
            if (currentAttack == 1)
            {
                StartCoroutine(Charge(new Vector2(AIPath.destination.x - transform.position.x, AIPath.destination.y - transform.position.y)));
            }

            // SPAWN
            else if (currentAttack == 2)
            {
                StartCoroutine(Spawn());
            }

            // GIGA CHARGE
            else 
            {
                StartCoroutine(GigaCharge(new Vector2(AIPath.destination.x - transform.position.x, AIPath.destination.y - transform.position.y)));
            }

            currentAttack = 0;
        }
    }

    public void FixedBeaverBehavior()
    {
        if (!isAttacking)
        {
            direction = AIPath.targetDirection;

            rb.AddForce(new Vector2(direction.x * speedX, direction.y * speedY) * 5, ForceMode2D.Force);
        }

        else if (isGigaCharging)
        {

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



    IEnumerator Charge(Vector2 directionJump)
    {
        isCharging = true;

        rb.AddForce(-directionJump.normalized * (strenghtJump / 5), ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.6f);

        rb.AddForce(directionJump.normalized * strenghtJump, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.25f);

        isAttacking = false;
        isGigaCharging = true;
        timer = Random.Range(cooldownMin, cooldownMax);
    }


    IEnumerator Spawn()
    {
        transform.DOShakePosition(1, 1);

        yield return new WaitForSeconds(1);

        int nbrEnnemies = Random.Range(minCastorSpawn, maxCastorSpawn);

        List<int> indexSelected = new List<int>();

        for (int i = 0; i < nbrEnnemies; i++)
        {
            int newIndex = Random.Range(0, bossRoom.spawnPoints.Count);

            while (indexSelected.Contains(newIndex))
            {
                newIndex = Random.Range(0, bossRoom.spawnPoints.Count);
            }

            Instantiate(castor, bossRoom.spawnPoints[newIndex].position, Quaternion.identity);
            indexSelected.Add(newIndex);
        }

        isAttacking = false;
        timer = Random.Range(cooldownMin, cooldownMax);
    }


    IEnumerator GigaCharge(Vector2 directionJump)
    {
        isGigaCharging = true;

        rb.AddForce(-directionJump.normalized * (strenghtGigaJump / 5), ForceMode2D.Impulse);

        yield return new WaitForSeconds(1f);

        rb.AddForce(directionJump.normalized * strenghtGigaJump, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.25f);

        isAttacking = false;
        isGigaCharging = false;
        timer = Random.Range(cooldownMin, cooldownMax);
    }



    public void TakeDamages(int degats, GameObject bullet)
    {
        currentHealth -= degats;

        healthBar.fillAmount = (float) currentHealth / health;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
