using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Effect", menuName = "Weapon/Damage Effect")]
public class DamageEffectDataSO : ScriptableObject
{
    [field: SerializeField] public float DamagePerSecond { get; private set; } = 0.2f;
    [field: SerializeField] public float LifeTime { get; private set; } = 3.0f;
    [field: SerializeField] public AudioCollection Sound { get; private set; }

    public DamageEffect CreateEffect(IDamageble damageble) => new(DamagePerSecond, LifeTime, Sound, damageble);
}
