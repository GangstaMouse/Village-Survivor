using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Range Weapon", menuName = "Weapons/Range Weapon")]
public class RangeWeapon : WeaponBase
{
    public float Speed => m_Speed + m_Owner.Stats.GetStat("Projectile Speed").Value;
    public float ProjectiveLifeTime => m_ProjectiveLifeTime + m_Owner.Stats.GetStat("Projectile Life Time").Value;
    public int PenetrationsAmount => m_PenetrationsAmount + (int)m_Owner.Stats.GetStat("Projectile Penetrations Amount").Value;
    [SerializeField] private float m_Speed = 5.0f;
    [SerializeField] private float m_ProjectiveLifeTime = 3.0f;
    [SerializeField] private int m_PenetrationsAmount = 1;

    [SerializeField] private GameObject m_ProjectilePrefab;

    [SerializeField] private WeaponShootModule m_ShootModule;

    protected override void OnAttack(in Character owner)
    {
        /* var projectileObject = Instantiate(m_ProjectilePrefab, owner.transform.position, quaternion.identity);
        var projectile = projectileObject.GetComponent<Projectile>();
        projectile.Init(owner, Damage, AttackRadius, owner.LookDirection * Speed, PenetrationsAmount, ProjectiveLifeTime);
        RegisterHitCallback(owner, projectile); */


        foreach (var projectile in m_ShootModule.Shoot(owner.transform.position, owner.LookDirection, m_ProjectilePrefab, this, owner, owner.AttackLayerMask))
            RegisterHitCallback(owner, projectile);
    }
}
