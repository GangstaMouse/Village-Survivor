using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public sealed class Inventoty : MonoBehaviour
{
    private Stats stats;
    [SerializeField] private List<StatModificator> m_Items = new();

    private void OnEnable() => stats = GetComponent<Character>().Stats;

    public void AddItem(StatModificator item)
    {
        m_Items.Add(item);
        stats.GetStat(item.StatID).AddModificator(item.GetMod());
    }

    public void RemoveItem(string key)
    {
        foreach (var item in m_Items)
            if (item.UpgradeID == key)
            {
                stats.GetStat(item.StatID).RemoveModificator(item.UpgradeID);
                m_Items.Remove(item);
            }
    }
}
