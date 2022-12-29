using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWallBoss : MonoBehaviour
{
    public BeaverBoss beaverBoss;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            beaverBoss.CollideWall();
        }
    }
}
