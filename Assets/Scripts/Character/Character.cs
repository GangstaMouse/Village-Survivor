using System;
using Unity.Mathematics;
using UnityEngine;

public abstract class Character : MonoBehaviour, IDamageble, IDamager
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

    public event Action<float> OnDamageTakenss;
    public static event Action OnDiedGlobal;
    public event Action OnDied;
    public static event Action<IDamager, Character> OnHit;

    public Vector2 LookVector;
    public Vector2 MoveVector;

    public AttackAbility AttackAbility;
    public float AttackCoolDown = 0.0f;

    protected Vector2 m_MovementInput;

    [Header("Sounds")]
    // Audio
    [SerializeField] AudioCollection DiedSound;
    [SerializeField] AudioCollection hurtSound;

    private void FixedUpdate()
    {
        if (IsAlive == false)
            return;
            
        OnFixedUpdate();

        AttackCoolDown = math.max(AttackCoolDown - Time.fixedDeltaTime, 0);

        m_MovementInput = Movement();
        LookVector = Looking();
        Debug.DrawLine(transform.position, transform.position + new Vector3(LookVector.x, LookVector.y, 1), Color.red);
        Vector2 movementVector = m_MovementInput * m_MovementSpeed * Time.fixedDeltaTime;

        
        transform.Translate(new(movementVector.x, movementVector.y, 0.0f));
    }

    protected void Attack()
    {
        if (IsAlive == false)
            return;

        if (AttackCoolDown == 0)
        {
            AttackAbility.Attack(this);
            AttackCoolDown = AttackAbility.CoolDown;
        }
    }

    protected abstract void OnKilled();

    public abstract void OnDamageTaken(float value);

    protected abstract void OnFixedUpdate();

    public void TakeDamage(IDamager damager, float value)
    {
        if (IsAlive == false)
            return;

        m_Health -= value;
        OnHit?.Invoke(damager, this);

        if (hurtSound)
            AudioManager.CreateAudioInstance(hurtSound, transform.position);

        if (Health <= 0.0f)
        {
            m_Health = 0.0f;
            Debug.Log($"{gameObject.name} - died");
            OnDiedGlobal?.Invoke();

            if (DiedSound)
                AudioManager.CreateAudioInstance(DiedSound, transform.position);
        }

        OnDamageTaken(value);
        OnDamageTakenss?.Invoke(Health);
    }

    protected abstract Vector2 Movement();
    protected abstract Vector2 Looking();
}
