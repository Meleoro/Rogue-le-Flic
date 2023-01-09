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
    [SerializeField] private float stunDuration;
    private float timer;
    private bool isAttacking;
    private bool canMove;
    private int currentAttack;
    private Vector2 direction;
    [HideInInspector] public bool lookLeft;
    private float stunTimer;

    [Header("Saut")]
    [SerializeField] private float distanceJumpTrigger;
    [SerializeField] private List<Transform> spotsJump;
    private Vector2 jumpDestination;
    private int cooldownJump;
    private Vector3 originalScale;
    
    [Header("Attaque sautée")]
    [SerializeField] private float duree;
    [SerializeField] private int minBoxSpawn;
    [SerializeField] private int maxBoxSpawn;
    [SerializeField] private GameObject box;

    [Header("Spawn")]
    [SerializeField] private int minFrogSpawn;
    [SerializeField] private int maxFrogSpawn;
    [SerializeField] private GameObject frog;
    private int cooldownSpawn;

    [Header("Tir")]
    public AnimationCurve tonguePatern;
    [SerializeField] private GameObject tongue;
    public float shotDuration;
    [HideInInspector] public bool recul;

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
                        currentAttack = Random.Range(1, 5);
                    }
                }
            }

            // ATTAQUE
            if (isAttacking && currentAttack != 0)
            {
                isAttacking = true;
                
                float distance = Mathf.Sqrt(Mathf.Pow(AIPath.destination.x - transform.position.x, 2) + 
                                            Mathf.Pow(AIPath.destination.y - transform.position.y, 2));

                if (distance < distanceJumpTrigger && cooldownJump <= 0)
                {
                    Jump(false);
                }
                
                else
                {
                    cooldownSpawn -= 1;
                    cooldownJump -= 1;

                    // ATTAQUE SAUTEE
                    if (currentAttack == 1)
                    {
                        Jump(true);
                    }

                    // SPAWN
                    else if (currentAttack == 5 && cooldownSpawn <= 0)
                    {
                        StartCoroutine(Spawn());
                    }

                    // TIR
                    else
                    {
                        StartCoroutine(Shoot());
                    }
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
            boss.anim.SetTrigger("reset");
        }
    }
    

    public void FixedFrogBehavior()
    {
        direction = AIPath.targetDirection;

        /*if (!isAttacking && stunTimer <= 0)
        {
            direction = AIPath.targetDirection;

            //rb.AddForce(new Vector2(direction.x * speedX, direction.y * speedY) * 5, ForceMode2D.Force);

            boss.anim.SetBool("isWalking", true);
        }

        else
        {
            boss.anim.SetBool("isWalking", false);
        }*/
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


    private void Jump(bool attaque)
    {
        float maxDistance = 0;
        cooldownJump = 2;

        for (int k = 0; k < spotsJump.Count; k++)
        {
            float newDistance = Vector2.Distance(ManagerChara.Instance.transform.position, spotsJump[k].position);

            if(newDistance > maxDistance)
            {
                jumpDestination = spotsJump[k].position;
                maxDistance = newDistance;
            }
        }

        if (!attaque)
            StartCoroutine(JumpChoroutine(jumpDestination));

        else
            StartCoroutine(JumpAttaqueChoroutine(ManagerChara.Instance.transform.position, jumpDestination));
    }

    
    IEnumerator JumpChoroutine(Vector2 destination)
    {
        StartCoroutine(Decollage(0.4f, 0.1f));

        yield return new WaitForSeconds(0.4f);
        
        boss.spawnIndicator.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = false;
        boss._collider2D.enabled = false;
        
        boss.sprite.transform.DOMoveY(transform.position.y + 20, 0.2f).SetEase(Ease.InCirc);
        
        boss.spawnIndicator.transform.DOScale(new Vector3(2f, 2f, 2f), 0);
        boss.spawnIndicator.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 1);

        transform.DOMove(destination, 2);
        
        yield return new WaitForSeconds(0.2f);

        boss.sprite.SetActive(false);
        
        yield return new WaitForSeconds(0.8f);
        
        boss.spawnIndicator.transform.DOScale(new Vector3(2f, 2f, 2f), 1f);

        yield return new WaitForSeconds(1f);
        
        boss.sprite.SetActive(true); 
        
        boss.sprite.transform.DOMoveY(transform.position.y + 1, 0.2f).SetEase(Ease.InCirc);;

        yield return new WaitForSeconds(0.2f);
        
        boss.spawnIndicator.SetActive(false);
        
        ReferenceCamera.Instance.transform.DOShakePosition(0.3f, 0.5f);
        
        GetComponent<BoxCollider2D>().enabled = true;
        boss._collider2D.enabled = true;
        
        isAttacking = false;
        timer = Random.Range(cooldownMin, cooldownMax);
    }


    IEnumerator JumpAttaqueChoroutine(Vector2 destination1, Vector2 destination2)
    {
        for(int i = 0; i < 2; i++)
        {
            StartCoroutine(Decollage(0.4f, 0.1f));

            yield return new WaitForSeconds(0.4f);
            
            boss.spawnIndicator.SetActive(true);
            GetComponent<BoxCollider2D>().enabled = false;
            boss._collider2D.enabled = false;

            if(i == 0)
            {
                boss.sprite.transform.DOMoveY(transform.position.y + 20, 0.1f).SetEase(Ease.InCirc);

                boss.spawnIndicator.transform.DOScale(new Vector3(2f, 2f, 2f), 0);
                boss.spawnIndicator.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f);

                transform.DOMove(destination1, 1);

                yield return new WaitForSeconds(0.1f);

                boss.sprite.SetActive(false);

                yield return new WaitForSeconds(0.4f);

                boss.spawnIndicator.transform.DOScale(new Vector3(2f, 2f, 2f), 0.5f);

                yield return new WaitForSeconds(0.5f);

                boss.sprite.SetActive(true);

                boss.sprite.transform.DOMoveY(transform.position.y + 1, 0.15f).SetEase(Ease.InCirc);
                
                yield return new WaitForSeconds(0.15f);

                ReferenceCamera.Instance.transform.DOShakePosition(0.7f, 0.6f);
                boss.spawnIndicator.SetActive(false);
                
                GetComponent<BoxCollider2D>().enabled = true;
                boss._collider2D.enabled = true;

                yield return new WaitForSeconds(0.5f);
            }

            else
            {
                boss.sprite.transform.DOMoveY(transform.position.y + 20, 0.2f).SetEase(Ease.InCirc);

                boss.spawnIndicator.transform.DOScale(new Vector3(2f, 2f, 2f), 0);
                boss.spawnIndicator.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 1);

                transform.DOMove(destination2, 2);

                yield return new WaitForSeconds(0.2f);

                boss.sprite.SetActive(false);

                yield return new WaitForSeconds(0.8f);

                boss.spawnIndicator.transform.DOScale(new Vector3(2f, 2f, 2f), 1);

                yield return new WaitForSeconds(1f);

                boss.sprite.SetActive(true);

                boss.sprite.transform.DOMoveY(transform.position.y + 1, 0.2f).SetEase(Ease.InCirc);

                yield return new WaitForSeconds(0.2f);
                
                boss.spawnIndicator.SetActive(false);
                
                ReferenceCamera.Instance.transform.DOShakePosition(0.3f, 0.4f);
            }

            GetComponent<BoxCollider2D>().enabled = true;
            boss._collider2D.enabled = true;
        }

        isAttacking = false;
        timer = Random.Range(cooldownMin, cooldownMax);
    }


    IEnumerator Shoot()
    {
        for(int k = 0; k < 3; k++)
        {
            boss.anim.SetTrigger("isAttacking");

            canMove = false;

            Vector3 direction = ManagerChara.Instance.transform.position - transform.position;
            Vector2 destination = ManagerChara.Instance.transform.position + direction.normalized * 3;

            transform.DOShakePosition(0.6f, 0.3f);

            yield return new WaitForSeconds(0.6f);

            GameObject currentTongue = Instantiate(tongue, transform.position, Quaternion.identity, transform);

            currentTongue.GetComponent<BossFrogTongue>().destination = destination;
            currentTongue.GetComponent<BossFrogTongue>().tongueDuration = shotDuration;

            yield return new WaitForSeconds(shotDuration);

            canMove = true;
        }

        //rb.AddForce(-direction.normalized, ForceMode2D.Impulse);

        isAttacking = false;
        timer = Random.Range(cooldownMin, cooldownMax);
    }


    public void Stun()
    {
        stunTimer = stunDuration;

        StopAllCoroutines();

        isAttacking = false;
        timer = Random.Range(cooldownMin, cooldownMax);

        boss.anim.SetTrigger("reset");
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

        if (bullet.CompareTag("Box"))
        {
            /*if (isIn)
            {
                recul = false;
                rb.AddForce((bullet.transform.position - transform.position).normalized * 20, ForceMode2D.Impulse);
            }*/
        }

        if (currentHealth <= 0)
        {
            boss.Death();
        }
    }


    IEnumerator Decollage(float duration1, float duration2)
    {
        originalScale = boss.sprite.transform.localScale;
        
        boss.sprite.transform.DOScale(new Vector3(originalScale.x + 0.1f, originalScale.y - 0.1f, originalScale.z), duration1);
        
        yield return new WaitForSeconds(duration1);
        
        boss.sprite.transform.DOScale(new Vector3(originalScale.x, originalScale.y, originalScale.z), duration2);
    }
}
