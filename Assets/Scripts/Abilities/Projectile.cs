using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_Radius = 0.5f;
    private Vector2 m_Velocity;
    [SerializeField] private LayerMask m_CollisionMask;
    IDamager m_Source;
    public int PenetrationsAmount;
    public int leftPens;

    public float m_Damage;

    public void Init(IDamager source, float damage, float radius, Vector2 initialVelocity, int pen)
    {
        m_Source = source;
        m_Damage = damage;
        m_Radius = radius;
        m_Velocity = initialVelocity;
        PenetrationsAmount = pen;
        leftPens = PenetrationsAmount;
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
                leftPens--;
                
                if (leftPens == 0)
                    Destroy(gameObject);
            }
        }
    }
}
