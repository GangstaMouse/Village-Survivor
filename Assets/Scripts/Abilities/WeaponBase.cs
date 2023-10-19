using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : ScriptableObject
{
    public virtual float Damage => m_Damage + m_Owner.Stats.GetStat("Damage").Value;
    public virtual float AttackRange => m_AttackRange + m_Owner.Stats.GetStat("Attack Range").Value;
    public virtual float AttackRadius => m_AttackRadius + m_Owner.Stats.GetStat("Attack Radius").Value;
    public virtual float Cooldown => m_CoolDown + m_Owner.Stats.GetStat("CoolDown").Value;

    [SerializeField] private float m_Damage = 2;
    [SerializeField] private float m_AttackRange = 1.4f;
    [SerializeField] private float m_AttackRadius = 0.5f;
    [SerializeField] private float m_CoolDown = 0.5f;

    [SerializeField] private AudioCollection attackSound;
    [SerializeField] protected List<WeaponEffect> m_Effects;

    protected Character m_Owner;

    public virtual void Init(in Character owner)
    {
        m_Owner = owner;
    }

    protected void RegisterHitCallback(Character owner, IDamager damager)
    {
        m_Owner = owner;
        damager.OnHitCallback += OnWeaponHit;
    }
    public void Attack(in Character owner)
    {
        OnAttack(owner);

        if (attackSound)
            AudioManager.CreateAudioInstance(attackSound, owner.transform.position);
    }
    protected abstract void OnAttack(in Character owner);

    private void OnWeaponHit(IDamageble damageble)
    {
        Debug.LogWarning("hit~!");
        Debug.Log($"Damage: {Damage}");

        // Applying Weapon Effects
        foreach (var effect in m_Effects)
            effect.DoEffect(m_Owner, damageble);
    }
}


