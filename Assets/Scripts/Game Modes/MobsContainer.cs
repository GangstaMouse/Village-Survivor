using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mobs Container", menuName = "Mobs Container")]
public class MobsContainer : ScriptableObject
{
    public List<MobSpawnData> Stages;
}

[Serializable]
public class MobSpawnData
{
    public GameObject MobPrefab;
    public List<int> AppearsInTheStages;
}
