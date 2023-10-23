using UnityEngine;

[CreateAssetMenu(fileName = "New Range Weapon", menuName = "Weapons/Range Weapon")]
public class RangeWeapon : WeaponDataSO
{
    [field: SerializeField] public float Speed { get; private set; } = 5.0f;
    [field: SerializeField] public float ProjectiveLifeTime { get; private set; } = 3.0f;
    [field: SerializeField] public int PenetrationsAmount { get; private set; } = 1;

    [field: SerializeField] public GameObject ProjectilePrefab { get; private set; }
    [field: SerializeField] public WeaponProjectileModule ShootModule { get; private set; }

    public override WeaponRuntime CreateRuntime(Character owner) => new RangeWeaponRuntime(owner, this);
}

public class RangeWeaponRuntime : WeaponRuntime
{
    public float Speed => m_Data.Speed + m_Owner.Stats.GetStat("Projectile Speed").Value;
    public float ProjectiveLifeTime => m_Data.ProjectiveLifeTime + m_Owner.Stats.GetStat("Projectile Life Time").Value;
    public int PenetrationsAmount => m_Data.PenetrationsAmount + (int)m_Owner.Stats.GetStat("Projectile Penetrations Amount").Value;
    protected new readonly RangeWeapon m_Data;

    public RangeWeaponRuntime(Character owner, RangeWeapon weaponData) : base(owner, weaponData)
    {
        m_Data = weaponData;
    }

    public override void OnAttack()
    {
        m_Data.ShootModule.CreateProjectiles(m_Owner.transform.position, m_Owner.LookDirection, m_Data, m_Owner.AttackLayerMask);
    }
}
