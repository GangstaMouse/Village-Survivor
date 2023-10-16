using System;
using System.Collections.Generic;

[Serializable]
public class ModificableValue
{
    public float BaseValue = 1;
    public float Value => GetValue();
    private float m_CachedValue;
    private bool m_IsCached = false;
    public List<Modificator> m_Modificators = new();

    /* public ModificableValue(string key)
    {
        
    } */

    public ModificableValue AddModificator(Modificator modificator)
    {
        m_IsCached = false;
        m_Modificators.Add(modificator);
        return this;
    }

    public ModificableValue RemoveModificator(Modificator modificator)
    {
        m_IsCached = false;
        m_Modificators.Remove(modificator);
        return this;
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

    public Modificator(float value) => Value = value;
}