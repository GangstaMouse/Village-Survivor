using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour, IDamageble, IInputReceiver
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

    public Vector2 LookDirection => InputHandler.LookingDirection;
    public Vector2 MovementInput => InputHandler.MovementInput;

    public float AttackCooldown = 0.0f;

    Vector3 IDamageble.Position => transform.position;
    float IDamageble.HealthPoints { get => m_Health; set { m_Health = value; } }
    List<DamageEffect> IDamageble.DamageEffects { get => m_Effects; }

    Action<float> IDamageble.OnHit => OnHit;
    Action IDamageble.OnDied => OnDied;

    public InputHandlerInstance InputHandler { get; private set; } = new();
    InputHandlerInstance IInputReceiver.InputHandler { get => InputHandler; set => InputHandler = value; }

    public event Action<float> OnHit;
    public event Action OnDied;

    [field: SerializeReference] private List<DamageEffect> m_Effects = new();

    private Rigidbody2D m_RigidBody;

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Debug.Log(InputHandler.MovementInput);
        if (IsAlive == false)
        {
            m_RigidBody.velocity = Vector2.zero;
            return;
        }

        AttackCooldown = math.max(AttackCooldown - Time.fixedDeltaTime, 0);

        // Movement
#if UNITY_EDITOR
        Debug.DrawLine(transform.position, transform.position + new Vector3(LookDirection.x, LookDirection.y, 1), Color.red);
#endif
        Vector2 movementVector = MovementInput * (MovementSpeed * Time.fixedDeltaTime);

        m_RigidBody.velocity = movementVector / Time.fixedDeltaTime;
    }

    private void LateUpdate()
    {
        transform.rotation = quaternion.AxisAngle(-math.forward(), math.atan2(LookDirection.x, LookDirection.y));
    }

    void IInputReceiver.OnInputHandlerChanged(in InputHandlerInstance inputHandler)
    {
    }
}
