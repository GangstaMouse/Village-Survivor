using System;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Weapons/Melee Weapon")]
public sealed class MeleeWeapon : WeaponBase, IDamager
{
    public event Action<IDamageble> OnHitCallback;

    public void Init(IDamageSource damageSource)
    {
        throw new NotImplementedException();
    }

    protected override void OnAttack(in IDamageSource damageSource)
    {
        RegisterHitCallback(damageSource, this);

        Ray2D ray = new(damageSource.Origin,
            math.normalizesafe(damageSource.Direction));

        Debug.Log($"Attack!");
        RaycastHit2D hitInfo;

        if (hitInfo = Physics2D.CircleCast(ray.origin, AttackRadius, ray.direction, AttackRange, damageSource.LayerMask))
        {
            if (hitInfo.collider.TryGetComponent(out IDamageble damageble))
            {
                damageble.TakeDamage(damageSource, Damage);
                OnHitCallback?.Invoke(damageble);
            }

            Debug.Log($"Damage: {Damage}");
        }
    }
}
