using System;
using UnityEngine;

public interface IDamageble
{
    public GameObject game => null;
    public bool IsAlive => false;
    public static event Action<IDamageSource, IDamageble> OnHit;

    public void TakeDamage(IDamageSource source, float value)
    {
        OnDamageTaken(source, value);
        // Notify systems about hit event
        OnHit?.Invoke(source, this);
    }

    protected abstract void OnDamageTaken(IDamageSource damageSource, float value);
}
