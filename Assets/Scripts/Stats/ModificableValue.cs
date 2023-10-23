using System;
using System.Collections.Generic;
using System.IO;

[Serializable]
public class ModificableValue
{
    public float BaseValue = 1.0f; // rework, need some thing else...
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

    public ModificableValue RemoveModificator(string key)
    {
        m_IsCached = false;
        foreach (var modificator in m_Modificators)
            if (modificator.Key == key)
                m_Modificators.Remove(modificator);
        return this;
    }

    private float GetValue()
    {
        if (m_IsCached)
            return m_CachedValue;

        float result = BaseValue;
        foreach (var mod in m_Modificators)
            result += GetModificatorValue(mod, BaseValue);

        m_CachedValue = result;
        m_IsCached = true;

        return result;
    }

    public float GetModificatorValue(in Modificator modificator, in float flowValue) => modificator.Type switch
    {
        ValueEditingType.Add       => flowValue + modificator.Value,
        ValueEditingType.Substract => flowValue - modificator.Value,
        ValueEditingType.Multiply  => flowValue * modificator.Value,
        ValueEditingType.Divide    => flowValue / modificator.Value,
        _ => throw new InvalidDataException("Invalid Modificator type")
    };
}
