using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public sealed class Stats
{
    /* public ModificableValue Health;
    public ModificableValue MaxHealth;
    public ModificableValue Armor;
    public ModificableValue Endurance;
    public ModificableValue Speed;
    public ModificableValue Damage; */

    private Dictionary<string, ModificableValue> m_Datas = new();

    public ModificableValue AddStat(string key)
    {
        ModificableValue newValue = new();
        m_Datas.Add(key, newValue);
        return newValue;
    }

    public ModificableValue GetStat(string key)
    {
        foreach (var stat in m_Datas)
            if (stat.Key == key)
                return stat.Value;

        Debug.LogWarning($"Stat: '{key}' not found! Creating new ones...");
        return AddStat(key);
    }
}

public abstract class Character : MonoBehaviour, IDamageble, IDamageSource
{
    public float Health => m_Health;
    public float MaxHealth => m_MaxHealth;
    public float Armor => m_Armor;
    public float Endurance => m_Endurance;
    public float MovementSpeed => m_MovementSpeed * stats.GetStat("Speed").Value;
    public Stats stats = new();

    public bool IsAlive => Health > 0.0f;

    [SerializeField] protected float m_Health = 5.0f;
    [SerializeField] protected float m_MaxHealth = 5.0f;
    [SerializeField] protected float m_Armor = 0.0f;
    [SerializeField] protected float m_Endurance = 12.0f;
    [SerializeField] protected float m_MovementSpeed = 3.4f;

    public LayerMask AttackLayerMask => m_AttackLayerMask;
    [SerializeField] private LayerMask m_AttackLayerMask;

    public event Action<IDamageSource, IDamageble> OnHitLocal;

    public abstract Vector2 LookDirection { get; }
    public abstract Vector2 MovementInput { get; }

    Vector2 IDamageSource.Origin => transform.position;
    Vector2 IDamageSource.Direction => LookDirection;
    LayerMask IDamageSource.LayerMask => AttackLayerMask;

    public WeaponBase Weapon;
    public float AttackCoolDown = 0.0f;

    [Header("Sounds")]
    // Audio
    [SerializeField] AudioCollection DiedSound;
    [SerializeField] AudioCollection hurtSound;

    GameObject IDamageble.game => gameObject;

    bool IDamageble.IsAlive => IsAlive;

    private void FixedUpdate()
    {
        if (IsAlive == false)
            return;
            
        OnFixedUpdate();

        AttackCoolDown = math.max(AttackCoolDown - Time.fixedDeltaTime, 0);

        // Movement
        Debug.DrawLine(transform.position, transform.position + new Vector3(LookDirection.x, LookDirection.y, 1), Color.red);
        Vector2 movementVector = MovementInput * (MovementSpeed * Time.fixedDeltaTime);

        transform.Translate(movementVector);
    }

    protected void Attack()
    {
        if (IsAlive == false)
            return;

        if (AttackCoolDown == 0)
        {
            Weapon.Attack(this);
            AttackCoolDown = Weapon.CoolDown;
        }
    }

    protected abstract void OnFixedUpdate();

    void IDamageble.OnDamageTaken(IDamageSource damageSource, float value)
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
    }
}
