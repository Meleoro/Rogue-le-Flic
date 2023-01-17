using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DegatsManager : MonoBehaviour
{
    public static DegatsManager Instance;

    public int degatsBox;
    public int degatsEnnemyIntoWall;
    public int degatsKickedEnnemy;

    public int degatsTurtleKicked;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
            Destroy(gameObject);
    }
}
