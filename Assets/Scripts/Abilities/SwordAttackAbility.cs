using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword Attack Ability", menuName = "Abilities/Attack/Sword")]
public sealed class SwordAttackAbility : AttackAbility
{
    protected override void OnAttack(in Character characterContext)
    {
        Ray2D ray = new(characterContext.transform.position,
            math.normalizesafe(characterContext.LookVector));

        Debug.Log($"Attack!");
        RaycastHit2D hitInfo;

        if (hitInfo = Physics2D.CircleCast(ray.origin, AttackRadius, ray.direction, AttackRange, characterContext.AttackLayerMask))
        {
            if (hitInfo.collider.TryGetComponent(out IDamageble damageble))
                damageble.TakeDamage(characterContext, Damage);

            if (hitInfo.collider.TryGetComponent(out Character character) && character.Health == 0)

            Debug.Log($"Damage: {Damage}");
        }
    }
}
