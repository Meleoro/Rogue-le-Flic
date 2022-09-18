using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
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
