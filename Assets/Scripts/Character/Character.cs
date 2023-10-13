using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IDamageble
{
    public float Health => m_Health;
    public float MaxHealth => m_MaxHealth;
    public float Armor => m_Armor;
    public float Endurance => m_Endurance;
    public float MovementSpeed => m_MovementSpeed;

    public bool IsAlive => Health > 0.0f;

    [SerializeField] protected float m_Health = 5.0f;
    [SerializeField] protected float m_MaxHealth = 5.0f;
    [SerializeField] protected float m_Armor = 0.0f;
    [SerializeField] protected float m_Endurance = 12.0f;
    [SerializeField] protected float m_MovementSpeed = 3.4f;

    public LayerMask AttackLayerMask => m_AttackLayerMask;
    [SerializeField] private LayerMask m_AttackLayerMask;

    public Vector2 LookVector;
    public Vector2 MoveVector;

    public AttackAbility AttackAbility;

    private Vector2 m_MovementInput;

    private void FixedUpdate()
    {
        if (IsAlive == false)
            return;

        m_MovementInput = Movement();
        LookVector = Looking();
        Debug.DrawLine(transform.position, transform.position + new Vector3(LookVector.x, LookVector.y, 1), Color.red);
        Vector2 movementVector = m_MovementInput * m_MovementSpeed * Time.fixedDeltaTime;

        
        transform.Translate(new(movementVector.x, movementVector.y, 0.0f));
    }

    protected void Attack() => AttackAbility.OnAttack(this);

    public abstract void OnDamageTaken(float value);

    public void TakeDamage(float value)
    {
        if (IsAlive == false)
            return;

        m_Health -= value;

        if (Health <= 0.0f)
        {
            m_Health = 0.0f;
            Debug.Log($"{gameObject.name} - died");
        }

        OnDamageTaken(value);
    }

    protected abstract Vector2 Movement();
    protected abstract Vector2 Looking();
}
