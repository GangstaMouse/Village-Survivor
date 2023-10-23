using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats Modificator", menuName = "Stats Modificator")]
public class StatModificator : ItemScriptableObject
{
    // Example - MaxHealth 5 Add / Total MaxHealth = BaseValue + Modificator
    [field: SerializeField] public string StatID { get; private set; }
    [field: SerializeField] public float Value { get; private set; } = 1.0f;
    [field: SerializeField] public ValueEditingType UpgradeEffect { get; private set; } = ValueEditingType.Add;
    [field: SerializeField] public string UpgradeID { get; private set; }

    public void ApplyModificator(Stats stats) => ApplyModificator(stats.GetStat(StatID));
    public void ApplyModificator(ModificableValue modificableValue) => modificableValue.AddModificator(new(UpgradeID, Value, UpgradeEffect));
    public Modificator GetMod() => new(UpgradeID, Value, UpgradeEffect);

    private void Reset() => UpgradeID = Guid.NewGuid().ToString();
}
