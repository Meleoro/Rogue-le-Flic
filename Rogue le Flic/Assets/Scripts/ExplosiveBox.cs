using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBox : MonoBehaviour
{
    [SerializeField] private CircleCollider2D explosionAura;


    public void Explose()
    {
        explosionAura.enabled = true;

        Destroy(gameObject, 0.1f);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Explose();
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ennemy"))
        {
            collision.GetComponent<Ennemy>().TakeDamages(5, gameObject);
        }
    }
}
