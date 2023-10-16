using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : ScriptableObject
{
    public float Damage = 2;
    public float AttackRange = 1.4f;
    public float AttackRadius = 0.5f;
    public float CoolDown = 0.5f;
    [SerializeField] private AudioCollection attackSound;
    [SerializeField] protected List<WeaponEffect> m_Effects;

    protected IDamageSource m_DamageSource;

    protected void RegisterHitCallback(IDamageSource damageSource, IDamager damager)
    {
        m_DamageSource = damageSource;
        damager.OnHitCallback += OnWeaponHit;
    }
    public void Attack(in IDamageSource damageSource)
    {
        m_DamageSource = damageSource;
        OnAttack(damageSource);

        if (attackSound)
            AudioManager.CreateAudioInstance(attackSound, damageSource.Origin);
    }
    protected abstract void OnAttack(in IDamageSource damageSource);

    private void OnWeaponHit(IDamageble damageble)
    {
        Debug.LogWarning("hit~!");

        // Applying Weapon Effects
        foreach (var effect in m_Effects)
            effect.DoEffect(m_DamageSource, damageble);
    }
}


