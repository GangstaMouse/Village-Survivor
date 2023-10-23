using System;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Weapons/Melee Weapon")]
public sealed class MeleeWeapon : WeaponDataSO
{
    public override WeaponRuntime CreateRuntime(Character owner) => new MeleeWeaponRuntime(owner, this);
}

public class MeleeWeaponRuntime : WeaponRuntime
{
    protected new readonly MeleeWeapon m_Data;

    public MeleeWeaponRuntime(Character owner, MeleeWeapon weaponData) : base(owner, weaponData)
    {
        m_Data = weaponData;
    }

    public override void OnAttack()
    {
        // RegisterHitCallback(this);

        Ray2D ray = new(m_Owner.transform.position,
            math.normalizesafe(m_Owner.LookDirection));

        ImpactSystem.Impact(new(){ Damage = Damage, effects = m_Data.DamageEffects }, Physics2D.CircleCast(ray.origin, AttackRadius, ray.direction, AttackRange, m_Owner.AttackLayerMask), OnHitEvent);

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
