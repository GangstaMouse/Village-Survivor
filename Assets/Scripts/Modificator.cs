using System;
using System.Collections.Generic;

[Serializable]
public class ModificableValue
{
    public float BaseValue = 1;
    public float Value => GetValue();
    private float m_CachedValue;
    private bool m_IsCached = false;
    private List<Modificator> m_Modificators = new();

    public void AddModificator(Modificator modificator)
    {
        m_IsCached = false;
        m_Modificators.Add(modificator);
    }

    public void RemoveModificator(Modificator modificator)
    {
        m_IsCached = false;
        m_Modificators.Remove(modificator);
    }

    public float GetValue()
    {
        if (m_IsCached)
            return m_CachedValue;

        float result = BaseValue;
        foreach (var mod in m_Modificators)
            result += mod.OutValue;

        m_CachedValue = result;
        m_IsCached = true;

        return result;
    }
}

[Serializable]
public class Modificator
{
    public string Key;
    public float OutValue => Value;
    public float Value = 1;
}