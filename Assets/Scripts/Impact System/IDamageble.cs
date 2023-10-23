using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    public Vector3 Position { get; }
    public float HealthPoints { get; protected internal set; }
    protected internal List<DamageEffect> DamageEffects { get; }

    protected internal void TakeDamage(float value)
    {
    }
}
