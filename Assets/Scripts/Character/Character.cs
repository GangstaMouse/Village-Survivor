using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Character : MonoBehaviour, IDamageble, IDamageSource
{
    public float Health => m_Health;
    public float MaxHealth => m_MaxHealth + m_Stats.GetStat("Max Health").Value;
    public float Armor => m_Armor;
    public float Endurance => m_Endurance;
    public float MovementSpeed => m_MovementSpeed + m_Stats.GetStat("Speed").Value;
    public Stats Stats => m_Stats;
    private Stats m_Stats = new();

    public bool IsAlive => Health > 0.0f;

    [SerializeField] protected float m_Health = 5.0f;
    [SerializeField] protected float m_MaxHealth = 5.0f;
    [SerializeField] protected float m_Armor = 0.0f;
    [SerializeField] protected float m_Endurance = 12.0f;
    [SerializeField] protected float m_MovementSpeed = 3.4f;

    public LayerMask AttackLayerMask => m_AttackLayerMask;
    [SerializeField] private LayerMask m_AttackLayerMask;
    [SerializeField] private LayerMask m_CollisionMask;

    public event Action<IDamageSource, IDamageble> OnHitLocal;

    public abstract Vector2 LookDirection { get; }
    public abstract Vector2 MovementInput { get; }

    Vector2 IDamageSource.Origin => transform.position;
    Vector2 IDamageSource.Direction => LookDirection;
    LayerMask IDamageSource.LayerMask => AttackLayerMask;

    public float AttackCooldown = 0.0f;

    [Header("Sounds")]
    // Audio
    [SerializeField] AudioCollection DiedSound;
    [SerializeField] AudioCollection hurtSound;

    /*     GameObject IDamageble.GameObject => gameObject;

        bool IDamageble.IsAlive => IsAlive; */

    Vector3 IDamageble.Position => transform.position;
    float IDamageble.HealthPoints { get => m_Health; set { m_Health = value; } }
    List<DamageEffect> IDamageble.DamageEffects { get => m_Effects; }
    [field: SerializeReference] private List<DamageEffect> m_Effects = new();

    private Rigidbody2D m_RigidBody;

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (IsAlive == false)
        {
            m_RigidBody.velocity = Vector2.zero;
            return;
        }

        OnFixedUpdate();

        AttackCooldown = math.max(AttackCooldown - Time.fixedDeltaTime, 0);

        // Movement
        Debug.DrawLine(transform.position, transform.position + new Vector3(LookDirection.x, LookDirection.y, 1), Color.red);
        Vector2 movementVector = MovementInput * (MovementSpeed * Time.fixedDeltaTime);

        // transform.Translate(movementVector);
        m_RigidBody.velocity = movementVector / Time.fixedDeltaTime;
    }

    protected void Attack()
    {
        if (IsAlive == false)
            return;

        /* if (AttackCooldown == 0)
        {
            Weapon.Attack(this);
            AttackCooldown = Weapon.Cooldown;
        } */
    }

    protected abstract void OnFixedUpdate();

    /* void IDamageble.OnDamageTaken(IDamageSource damageSource, float value)
    {
        if (IsAlive == false)
            return;

        m_Health -= value;

        if (hurtSound)
            AudioManager.CreateAudioInstance(hurtSound, transform.position);

        if (Health <= 0.0f)
        {
            m_Health = 0.0f;
            Debug.Log($"{gameObject.name} - died");

            if (DiedSound)
                AudioManager.CreateAudioInstance(DiedSound, transform.position);
        }

        OnHitLocal?.Invoke(damageSource, this);
    } */
}
