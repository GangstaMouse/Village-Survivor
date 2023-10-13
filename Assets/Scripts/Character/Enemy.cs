using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;

public class Enemy : Character
{
    private void OnEnable()
    {
        Character character = GetComponent<Character>();
    }

    /* public float Damage = 0.7f;
    public float AttackRange = 0.9f;

    private void FixedUpdate()
    {
        if (Player.Instance == null && Player.Instance.IsAlive)
            return;

        float distanceToPlayer = math.length(Player.Instance.transform.position - transform.position);

        float3 m_MovementInput3D = math.normalizesafe(Player.Instance.transform.position - transform.position);
        m_MovementInput = new float2(m_MovementInput3D.x, m_MovementInput3D.y);

        m_MovementVector = m_MovementInput * (MovementSpeed * Time.fixedDeltaTime);
        transform.Translate(new float3(m_MovementVector.x, m_MovementVector.y, 0.0f));

        float deltaDamage = Damage; //* Time.fixedDeltaTime;

        if (distanceToPlayer <= AttackRange)
            Player.Instance.TakeDamage(deltaDamage);
    } */

    public override void OnDamageTaken(float value)
    {
        // throw new System.NotImplementedException();
    }

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
}
