using System;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class LevelComponent : MonoBehaviour
{
    [field: SerializeField] public int Level { get; private set; } = 1;
    [field: SerializeField] public int Experience { get; private set; }
    public static event Action<int> OnExperienceChanged;
    public static event Action<int> OnLevelChanged;
    private IDamageble m_Player;
    private Action<IDamageble> m_OnDiedFunc;

    private void Awake()
    {
        m_Player = GetComponent<Player>();

        m_OnDiedFunc = (damagble) =>
        {
            if (damagble != m_Player)
                IncreaseExperience();
        };
    }

    private void IncreaseExperience()
    {
        Experience++;

        if (Experience >= 10)
        {
            Level += Experience / 10;
            Experience %= 10;
            OnLevelChanged?.Invoke(Level);
        }

        OnExperienceChanged?.Invoke(Experience);
    }
    private void OnEnable() => ImpactSystem.OnDiedEvent += (damageble) => m_OnDiedFunc(damageble);

    private void OnDisable() => ImpactSystem.OnDiedEvent -= (damageble) => m_OnDiedFunc(damageble);
}
