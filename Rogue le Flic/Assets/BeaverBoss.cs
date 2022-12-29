using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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

        yield return new WaitForSeconds(1);

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
}
