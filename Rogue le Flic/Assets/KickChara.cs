using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

public class KickChara : MonoBehaviour
{
    public static KickChara Instance;

    public LayerMask ennemies;
    
    public float kickDuration;
    public float kickReach;
    public float kickStrenght;


    public void Awake()
    {
        Instance = this;
    }
    

    public void Kick()
    {
        Collider2D[] ennemiesTouched = Physics2D.OverlapCircleAll(transform.position, kickReach, ennemies);

        
        foreach (Collider2D k in ennemiesTouched)
        {
            Vector2 direction = transform.position - k.transform.position;
            
            k.gameObject.GetComponent<Rigidbody2D>().AddForce(-direction.normalized * kickStrenght, ForceMode2D.Impulse);
        }
    }
}
