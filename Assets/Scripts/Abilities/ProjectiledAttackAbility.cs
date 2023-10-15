using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
[CreateAssetMenu(fileName = "Sword Attack Ability", menuName = "Abilities/Attack/Projectile")]
public class ProjectiledAttackAbility : AttackAbility
{
    public float Speed = 5.0f;
    public float LifeTime = 3.0f;
    public int PenetrationsAmount = 1;
    public GameObject projectilePrefab;

    protected override void OnAttack(in Character characterContext)
    {
        Vector2 direction = characterContext.LookVector;
        var projectileObject = Instantiate(projectilePrefab, characterContext.transform.position, quaternion.identity);
        projectileObject.GetComponent<Projectile>().Init(characterContext, Damage, AttackRadius, direction * Speed, PenetrationsAmount);
        Destroy(projectileObject, LifeTime);
    }
}
