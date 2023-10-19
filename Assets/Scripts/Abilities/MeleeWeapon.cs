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

    protected override void OnAttack(in Character owner)
    {
        RegisterHitCallback(owner, this);

        Ray2D ray = new(owner.transform.position,
            math.normalizesafe(owner.LookDirection));

        Debug.Log($"Attack!");
        RaycastHit2D hitInfo;

        if (hitInfo = Physics2D.CircleCast(ray.origin, AttackRadius, ray.direction, AttackRange, owner.AttackLayerMask))
        {
            if (hitInfo.collider.TryGetComponent(out IDamageble damageble))
            {
                damageble.TakeDamage(owner, Damage);
                OnHitCallback?.Invoke(damageble);
            }

            Debug.Log($"Damage: {Damage}");
        }
    }
}
