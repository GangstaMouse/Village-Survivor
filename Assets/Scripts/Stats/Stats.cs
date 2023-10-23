using System.Collections.Generic;

public sealed class Stats
{
    private Dictionary<string, ModificableValue> m_Datas = new();

    public ModificableValue AddStat(string key)
    {
        ModificableValue newValue = new();
        m_Datas.Add(key, newValue);
        return newValue;
    }

    public ModificableValue GetStat(string key)
    {
        foreach (var stat in m_Datas)
            if (stat.Key == key)
                return stat.Value;

        // Debug.LogWarning($"Stat: '{key}' not found! Creating new ones...");
        return AddStat(key);
    }
}
