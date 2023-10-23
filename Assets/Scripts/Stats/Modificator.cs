using System;

[Serializable]
public class Modificator
{
    public readonly string Key;
    public readonly float Value;
    public readonly ValueEditingType Type;

    public Modificator(in string key, in float value, in ValueEditingType type)
    {
        Key = key;
        Value = value;
    }
}
