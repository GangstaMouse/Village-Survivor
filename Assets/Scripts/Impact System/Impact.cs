using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public struct Impact
{
    public float3 Direction;
    public float3 Point;
}

public struct AdvancedImpact
{
    /* public Character Damager;
    public float3 FromDirection;
    public float3 FromPoint;
    public Character Damaged;
    public float3 ToDirection;
    public float3 ToPoint; */
}

public struct ImpactInputData
{
    public float Damage;
    public List<DamageEffectDataSO> effects;
}

public static class ImpactSystem
{
    public delegate void OnImpact(IDamageble damageble);
    public static event OnImpact OnImpactEvent;
    public static event OnImpact OnDiedEvent;

    public static void Impact(in ImpactInputData context, in RaycastHit2D impactInfo, in OnImpact impactCallback)
    {
        if (impactInfo.collider != null && impactInfo.collider.TryGetComponent(out IDamageble damageble))
            ImpactProcess(context, damageble, impactCallback);
    }

    public static void Impact(in ImpactInputData context, in Collider2D impactCollider, in OnImpact impactCallback)
    {
        if (impactCollider != null && impactCollider.TryGetComponent(out IDamageble damageble))
            ImpactProcess(context, damageble, impactCallback);
    }

    public static void Impact(in ImpactInputData context, in IDamageble damageble, in OnImpact impactCallback)
    {
        if (damageble != null)
            ImpactProcess(context, damageble, impactCallback);
    }

    private static void ImpactProcess(in ImpactInputData context, IDamageble damageble, in OnImpact impactCallback)
    {
        bool wasAlive = damageble.HealthPoints > 0;
        damageble.HealthPoints = math.max(damageble.HealthPoints - context.Damage, 0);

        foreach (var effect in context.effects)
            damageble.DamageEffects.Add(effect.CreateEffect(damageble));

        damageble.OnHit?.Invoke(damageble.HealthPoints);
        OnImpactEvent?.Invoke(damageble);

        if (wasAlive && damageble.HealthPoints == 0)
        {
            OnDiedEvent?.Invoke(damageble);
            damageble.OnDied?.Invoke();
        }
            
        impactCallback?.Invoke(damageble);
    }

    internal static void OnEffectLifeTimeOver(DamageEffect damageEffect, IDamageble damageble)
    {
        Debug.LogWarning(damageble.DamageEffects.Count);
        damageble.DamageEffects.Remove(damageEffect);
        Debug.Log(damageble.DamageEffects.Count);
    }


    /* private Impact GenerateNewImpact(RaycastHit2D impactInfo)
    {
        return new()
    } */
}
