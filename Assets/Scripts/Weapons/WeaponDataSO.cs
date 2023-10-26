using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class WeaponDataSO : ItemScriptableObject
{
    [field: SerializeField] public float Damage { get; private set; } = 2;
    [field: SerializeField] public float AttackRange { get; private set; } = 1.4f;
    [field: SerializeField] public float AttackRadius { get; private set; } = 0.5f;
    [field: SerializeField] public float Cooldown { get; private set; } = 0.5f;
    [field: SerializeField] public bool AutoAttack { get; private set; } = false;

    [field: SerializeField] public AudioCollection AttackSound { get; private set; }
    // Need also to add particles
    [field: SerializeField] public AudioCollection HitSound { get; private set; }
    [field: SerializeField] public List<DamageEffectDataSO> DamageEffects { get; private set; }
    [field: SerializeField] public WeaponAttackModule AttackModule { get; private set; }

    public abstract WeaponRuntime CreateRuntime(Character owner);
}

public abstract class WeaponRuntime // : IDamager
{
    public float Damage => m_Data.Damage + m_Owner.Stats.GetStat("Damage").Value;
    public float AttackRange => m_Data.AttackRange + m_Owner.Stats.GetStat("Attack Range").Value;
    public float AttackRadius => m_Data.AttackRadius + m_Owner.Stats.GetStat("Attack Radius").Value;
    public float Cooldown => m_Data.Cooldown + m_Owner.Stats.GetStat("Cooldown").Value;
    protected readonly Character m_Owner;
    protected readonly WeaponDataSO m_Data;

    protected ImpactSystem.OnImpact OnHitEvent;

    private float cooldownValue = 0.0f;
    private bool m_IsAttacking = false;
    private bool m_PrevState = false;
    private bool canAttack = true;

    public WeaponRuntime(Character owner, WeaponDataSO weaponData)
    {
        m_Owner = owner;
        m_Data = weaponData;
        if (m_Data.HitSound)
            OnHitEvent = PlayerHitSound;
    }

    private void PlayerHitSound(IDamageble damageble)
    {
        AudioManager.CreateAudioInstance(m_Data.HitSound, damageble.Position);
    }

    public void Attack()
    {
        m_PrevState = m_IsAttacking;
        m_IsAttacking = true;
    }

    public void Release()
    {
        m_PrevState = m_IsAttacking;
        m_IsAttacking = false;
    }

    public void Tick(float deltaTime)
    {
        /* if (m_IsAttacking == true && m_Data.AutoAttack ||
            (m_IsAttacking == true && m_Data.AutoAttack == false && m_PrevState != m_IsAttacking)) */
        if (m_IsAttacking && canAttack)
            Logic();

        cooldownValue = math.max(cooldownValue - deltaTime, 0.0f);
    }

    protected virtual void Logic()
    {
        if (cooldownValue != 0)
            return;

        canAttack = false;
        m_Data.AttackModule.Initiate(Attk, OnAttackOver);
    }

    private void Attk()
    {
        OnAttack();

        if (m_Data.AttackSound)
            AudioManager.CreateAudioInstance(m_Data.AttackSound, m_Owner.transform.position);
    }

    public abstract void OnAttack();

    private void OnAttackOver()
    {
        canAttack = true;
        cooldownValue = Cooldown;
    }
}
