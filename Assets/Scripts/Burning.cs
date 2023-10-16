using System;
using UnityEngine;

public class Burning : MonoBehaviour, IDamager
{
    public float damage = 0.9f;
    public float BurningTime = 3.0f;
    private IDamageble M_Damageble;
    private IDamageSource m_Source;

    public event Action<IDamageble> OnHitCallback;

    private void Awake() => M_Damageble = GetComponent<IDamageble>();
    public void Init(IDamageSource damageSource) => m_Source = damageSource;

    private void FixedUpdate()
    {
        M_Damageble.TakeDamage(m_Source, damage * Time.fixedDeltaTime);
        OnHitCallback?.Invoke(M_Damageble);

        BurningTime -= Time.fixedDeltaTime;

        if (BurningTime <= 0.0f)
            Destroy(this);
    }
}
