using System;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Weapons/Melee Weapon")]
public sealed class MeleeWeapon : WeaponDataSO
{
    public override WeaponRuntime CreateRuntime(IDamagerDirect damager) => new MeleeWeaponRuntime(damager, this);
}

public class MeleeWeaponRuntime : WeaponRuntime
{
    protected new readonly MeleeWeapon m_Data;

    public MeleeWeaponRuntime(IDamagerDirect damager, MeleeWeapon weaponData, Stats stats = default) : base(damager, weaponData, stats)
    {
        m_Data = weaponData;
    }

    public override void OnAttack()
    {
        // RegisterHitCallback(this);

        Ray2D ray = new(new float2(m_Damager.Origin.x, m_Damager.Origin.y),
            math.normalizesafe(new float2(m_Damager.Direction.x, m_Damager.Direction.y)));

        ImpactSystem.Impact(new ImpactInputData(){ Damage = Damage, effects = m_Data.DamageEffects }, Physics2D.CircleCast(ray.origin, AttackRadius, ray.direction, AttackRange, m_Damager.LayerMask), OnHitEvent);

        /* Debug.Log($"Attack!");
        RaycastHit2D hitInfo;

        if (hitInfo = Physics2D.CircleCast(ray.origin, AttackRadius, ray.direction, AttackRange, m_Owner.AttackLayerMask))
        {
            if (hitInfo.collider.TryGetComponent(out IDamageble damageble))
            {
                damageble.TakeDamage(m_Owner, Damage);
                // OnHitCallback?.Invoke(damageble);
            }

            Debug.Log($"Damage: {Damage}");
        } */
    }
}
