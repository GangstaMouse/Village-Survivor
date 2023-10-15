using UnityEngine;
using Unity.Mathematics;

public class Enemy : Character
{
    private void OnEnable()
    {
        Character character = GetComponent<Character>();
    }

    public override void OnDamageTaken(float value) { }

    protected override Vector2 Movement()
    {
        if (Player.Instance == null && Player.Instance.IsAlive)
            return Vector2.zero;

        float3 movementInput3D = math.normalizesafe(Player.Instance.transform.position - transform.position);
        return new(movementInput3D.x, movementInput3D.y);
    }

    protected override Vector2 Looking()
    {
        if (Player.Instance == null && Player.Instance.IsAlive)
            return Vector2.zero;

        float3 relativePlayerPosition = Player.Instance.transform.position - transform.position;
        return math.normalizesafe(new float2(relativePlayerPosition.x, relativePlayerPosition.y)); // test
    }

    protected override void OnFixedUpdate()
    {
        if (Player.Instance == null || Player.Instance.IsAlive == false)
            return;

        float distanceToPlayer = math.length(Player.Instance.transform.position - transform.position);

        if (distanceToPlayer <= AttackAbility.AttackRange)
            Attack();
    }

    protected override void OnKilled() { }
}
