using System;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    public abstract GameObject GameObject { get; }
    public abstract float Health { get; }
    public bool IsAlive => Health > 0.0f;
    public static event Action<IDamageSource, IDamageble> OnHit;

    public abstract List<int> effects { get; }

    /* public void DoDamage(Effects effects)
    {

    } */

    public void TakeDamage(IDamageSource source, float value)
    {
        OnDamageTaken(source, value);
        // Notify systems about hit event
        OnHit?.Invoke(source, this);
    }

    protected abstract void OnDamageTaken(IDamageSource damageSource, float value);
}
