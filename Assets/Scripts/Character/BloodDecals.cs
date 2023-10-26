using System;
using UnityEngine;

public interface IDecal
{
    // public event Action OnCreated;
    public float LifeTime { get; }
}

[RequireComponent(typeof(Character))]
public class BloodDecals : MonoBehaviour, IDecal
{
    [field: SerializeField] public DecalDataSO DecalData { get; private set; }

    [SerializeField] private float m_LifeTime = 2.5f;
    public float LifeTime => m_LifeTime;

    private Character m_Character;

    private void Awake() => m_Character = GetComponent<Character>();

    private void OnEnable()
    {
        if (!m_Character.IsAlive)
            return;

        m_Character.OnHit += OnHit;
    }

    private void OnDisable()
    {
        m_Character.OnHit -= OnHit;
    }

    private void OnHit(float health)
    {
        DecalManager.CreateInstance(DecalData, transform.position, health);
    }
}
