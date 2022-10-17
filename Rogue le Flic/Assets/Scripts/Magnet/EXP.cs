using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EXP : MonoBehaviour
{
    public static event Action OnExCollected;
    Rigidbody2D rb;

    public float valeurXp;

    bool hasTarget;
    Vector3 targetPosition;
    private float moveSpeed = 10f;

    private void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //valeurXp = ComboManager.Instance.currentMultiplier;
    }

    /*public override void Collect()
    {
        ExpBar.Instance.currentXp += valeurXp - 1;
        
        ScoreManager.Instance.AddPoint((int) valeurXp);
        
        Destroy(gameObject);
    }
    
    public interface ICollectible
    {
        public void Collect()
        {
            Debug.Log("it works now");
        }
        
    }*/

    private void FixedUpdate()
    {
        if (hasTarget)
        {
            Vector2 targetDirection = targetPosition - transform.position;
            rb.velocity = targetDirection.normalized * moveSpeed;
        }
    }

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTarget = true;
    }
}
