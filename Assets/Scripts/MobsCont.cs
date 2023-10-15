using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mobs Container", menuName = "Mobs Container")]
public class MobsCont : ScriptableObject
{
    public List<Stagess> Stages;
}

[Serializable]
public class Stagess
{
    public int stage;
    public List<GameObject> mobs;
}
