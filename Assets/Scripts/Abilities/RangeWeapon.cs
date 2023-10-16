using Unity.Mathematics;
using UnityEngine;
[CreateAssetMenu(fileName = "New Range Weapon", menuName = "Weapons/Range Weapon")]
public class RangeWeapon : WeaponBase
{
    public float Speed = 5.0f;
    public float LifeTime = 3.0f;
    public int PenetrationsAmount = 1;
    [SerializeField] private GameObject m_ProjectilePrefab;

    protected override void OnAttack(in IDamageSource damageSource)
    {
        m_DamageSource = damageSource;

        var projectileObject = Instantiate(m_ProjectilePrefab, damageSource.Origin, quaternion.identity);
        var projectile = projectileObject.GetComponent<Projectile>();
        projectile.Init(damageSource, Damage, AttackRadius, damageSource.Direction * Speed, PenetrationsAmount, LifeTime);
        RegisterHitCallback(damageSource, projectile);
    }
}
