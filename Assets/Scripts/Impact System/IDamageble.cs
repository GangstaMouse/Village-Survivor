using System;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    public Vector3 Position { get; }
    public float HealthPoints { get; protected internal set; }
    protected internal List<DamageEffect> DamageEffects { get; }
    protected internal Action<float> OnHit { get; }
    protected internal Action OnDied { get; }
}
