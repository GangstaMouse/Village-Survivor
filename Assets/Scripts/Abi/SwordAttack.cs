using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword Attack Ability", menuName = "Abilities/Attack/Sword")]
public sealed class SwordAttack : AttackAbility
{
    public float Damage = 2;
    public float AttackRange = 1.4f;
    public float AttackRadius = 0.5f;
    public override void OnAttack(in Character characterContext)
    {
        Ray2D ray = new(characterContext.transform.position,
            math.normalizesafe(characterContext.LookVector));

        Debug.Log($"Attack!");
        RaycastHit2D hitInfo;

        if (hitInfo = Physics2D.CircleCast(ray.origin, AttackRadius, ray.direction, AttackRange, characterContext.AttackLayerMask))
        {
            if (hitInfo.collider.TryGetComponent(out IDamageble damageble))
                damageble.TakeDamage(Damage);

            Debug.Log($"Damage: {Damage}");
        }
    }
}
