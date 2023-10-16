using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamager
{
    [SerializeField] private float m_Radius = 0.5f;
    private Vector2 m_Velocity;
    [SerializeField] private LayerMask m_CollisionMask;
    IDamageSource m_Source;
    public int PenetrationsAmount;
    public int leftPens;

    public float m_Damage;
    public List<WeaponEffect> effects;

    public event Action<IDamageble> OnHitCallback;

    public void Init(IDamageSource damageSource)
    {
        m_Source = damageSource;
    }

    public void Init(IDamageSource source, float damage, float radius, Vector2 initialVelocity, int pen, float lifeTime)
    {
        m_Source = source;
        m_Damage = damage * ((Character)source).stats.GetStat("Damage").Value;
        m_Radius = radius;
        m_Velocity = initialVelocity;
        PenetrationsAmount = pen;
        leftPens = PenetrationsAmount;
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(m_Velocity * Time.fixedDeltaTime);
        Collider2D collider;

        if (collider = Physics2D.OverlapCircle(transform.position, m_Radius, m_CollisionMask))
        {
            if (collider.gameObject.TryGetComponent(out IDamageble damageble))
            {
                damageble.TakeDamage(m_Source, m_Damage);
                OnHitCallback?.Invoke(damageble);
                leftPens--;
                
                if (leftPens == 0)
                    Destroy(gameObject);
            }
        }
    }
}
