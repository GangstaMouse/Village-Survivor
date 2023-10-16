using UnityEngine;
using Unity.Mathematics;

public class Enemy : Character
{
    public override Vector2 LookDirection { get
    {
        if (Player.Instance == null && Player.Instance.IsAlive)
            return Vector2.zero;

        float3 relativePlayerPosition = Player.Instance.transform.position - transform.position;
        return math.normalizesafe(new float2(relativePlayerPosition.x, relativePlayerPosition.y)); // test
    }}

    public override Vector2 MovementInput { get
    {
        if (Player.Instance == null && Player.Instance.IsAlive)
            return Vector2.zero;

        float3 movementInput3D = math.normalizesafe(Player.Instance.transform.position - transform.position);
        return new(movementInput3D.x, movementInput3D.y);
    }}
    
    protected override void OnFixedUpdate()
    {
        if (Player.Instance == null || Player.Instance.IsAlive == false)
            return;

        float distanceToPlayer = math.length(Player.Instance.transform.position - transform.position);

        if (distanceToPlayer <= Weapon.AttackRange)
            Attack();
    }
}
