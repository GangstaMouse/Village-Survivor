using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float m_Damage;
    private float m_Radius;
    private float3 m_Velocity;
    private int m_PenetrationsAmount;
    private float m_LifeTime;
    private LayerMask m_CollisionMask;
    // IDamageSource m_Source;

    public List<DamageEffectDataSO> damageEffects;
    // private event ImpactSystem.OnImpact OnHitCallback;

    public void Init(in float damage, in float radius, in float3 velocity, in int penetrationsAmount, in float lifeTime, in LayerMask layerMask)
    {
        m_Damage = damage;
        m_Radius = radius;
        m_Velocity = velocity;
        m_PenetrationsAmount = penetrationsAmount;
        m_CollisionMask = layerMask;
        m_LifeTime = lifeTime;
        
        transform.localScale = Vector3.one * radius;
    }

    private void FixedUpdate()
    {
        // add collider to ignore list

        ImpactSystem.Impact(new() { Damage = m_Damage, effects = damageEffects }, Physics2D.OverlapCircle(transform.position, m_Radius, m_CollisionMask), OnHit);
        
        m_LifeTime -= Time.fixedDeltaTime;

        if (m_LifeTime <= 0)
            Destroy(gameObject);

        transform.Translate(m_Velocity * Time.fixedDeltaTime);
    }

    private void OnHit(IDamageble damageble)
    {
        m_PenetrationsAmount--;
            
        if (m_PenetrationsAmount == 0)
            Destroy(gameObject);
    }
}
