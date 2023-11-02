using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Projectile Module", menuName = "Weapon Modules/Projectile Module")]
public sealed class WeaponProjectileModule : ScriptableObject
{
    [SerializeField] private int m_ProjectilesPerShoot = 1;
    [SerializeField] private float m_ShootAngle = 0.0f;

    public Projectile[] CreateProjectiles(in float3 origin, in float3 direction, in RangeWeapon weaponData, LayerMask collisionLayerMask)
    {
        Projectile[] projectiles = new Projectile[m_ProjectilesPerShoot];

        float deltaAngle = 0.0f;
        float initialAngle = 0.0f;

        if (m_ProjectilesPerShoot > 1)
        {
            deltaAngle = m_ShootAngle / (m_ProjectilesPerShoot - 1) * 2.0f;
            initialAngle = -deltaAngle * ((m_ProjectilesPerShoot - 1) / 2.0f);
        }

        for (int i = 0; i < m_ProjectilesPerShoot; i++)
        {
            float3 deltaDirection = math.mul(quaternion.AxisAngle(math.forward(),
                math.radians(initialAngle + (deltaAngle * i))), new float3(direction.x, direction.y, 0));

            projectiles[i] = CreateProjectileInstance(origin, deltaDirection, weaponData, collisionLayerMask);
        }

        return projectiles;
    }

    private Projectile CreateProjectileInstance(float3 origin, float3 direction, in RangeWeapon weaponData, LayerMask layerMask)
    {
        GameObject instance = Instantiate(weaponData.ProjectilePrefab, origin, quaternion.identity);
        Projectile projectile = instance.GetComponent<Projectile>();
        projectile.Init(weaponData.Damage, weaponData.AttackRadius,
            direction * weaponData.Speed, weaponData.PenetrationsAmount, weaponData.ProjectiveLifeTime, layerMask);
        return projectile;
    }
#if UNITY_EDITOR

    public void Debug(float3 pos)
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
            Gizmos.DrawLine(pos, pos + dir);
        }
    }
#endif
}
