using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Shoot Module", menuName = "Weapon Modules/Shoot Module")]
public sealed class WeaponShootModule : ScriptableObject
{
    [SerializeField] private int m_ProjectilesPerShoot = 1;
    [SerializeField] private float m_ShootAngle = 0.0f;

    public Projectile[] Shoot(Vector2 origin, Vector2 direction, GameObject prefab, in RangeWeapon weapon, IDamageSource damageSource, LayerMask layerMask)
    {
        Projectile[] projectiles = new Projectile[m_ProjectilesPerShoot];

        float deltaAngle = 0.0f;
        float startAngle = 0.0f;

        if (m_ProjectilesPerShoot > 1)
        {
            deltaAngle = m_ShootAngle / (m_ProjectilesPerShoot - 1) * 2.0f;
            startAngle = -deltaAngle * ((m_ProjectilesPerShoot - 1) / 2.0f);
        }

        for (int i = 0; i < m_ProjectilesPerShoot; i++)
        {
            float3 dir = math.mul(quaternion.AxisAngle(math.forward(), math.radians(startAngle + (deltaAngle * i))),
                new float3(direction.x, direction.y, 0));
            Vector2 dires = new(dir.x, dir.y);

            projectiles[i] = CreateProjectile(origin, dires, prefab, weapon, damageSource, layerMask);
        }

        return projectiles;
    }

#if UNITY_EDITOR
    public void Debug(Vector3 pos)
    {
        float deltaAngle = 0.0f;
        float startAngle = 0.0f;

        if (m_ProjectilesPerShoot > 1)
        {
            deltaAngle = m_ShootAngle / (m_ProjectilesPerShoot - 1) * 2.0f;
            startAngle = -deltaAngle * ((m_ProjectilesPerShoot - 1) / 2.0f);
        }

        // UnityEngine.Debug.LogWarning($"{deltaAngle}  {startAngle}");

        for (int i = 0; i < m_ProjectilesPerShoot; i++)
        {
            float3 dir = math.mul(quaternion.AxisAngle(math.forward(), math.radians(startAngle + (deltaAngle * i))), new float3(0, 1.0f, 0));
            Gizmos.DrawLine(pos, pos + (Vector3)dir);
        }
    }
#endif
    private Projectile CreateProjectile(Vector2 origin, Vector2 direction, GameObject prefab, in RangeWeapon weapon, IDamageSource damageSource, LayerMask layerMask)
    {
        var projectileObject = Instantiate(prefab, origin, quaternion.identity);
        var projectile = projectileObject.GetComponent<Projectile>();
        projectile.Init(damageSource, weapon.Damage, weapon.AttackRadius,
            direction * weapon.Speed, weapon.PenetrationsAmount, weapon.ProjectiveLifeTime, layerMask);
        return projectile;
    }
}
