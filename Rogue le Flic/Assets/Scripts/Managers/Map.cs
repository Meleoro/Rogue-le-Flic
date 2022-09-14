using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ligne
{
    public List<GameObject> list;
}

[System.Serializable]
public class Map
{
    public List<Ligne> list;
}