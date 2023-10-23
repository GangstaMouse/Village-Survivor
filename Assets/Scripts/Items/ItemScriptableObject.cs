using UnityEngine;

public abstract class ItemScriptableObject : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
}
