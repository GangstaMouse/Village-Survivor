using UnityEngine;

[CreateAssetMenu(fileName = "New Decal", menuName = "Village Survivor/Decal")]
public class DecalDataSO : ScriptableObject
{
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Color Color { get; private set; } = Color.white;
}
