using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterSounds : MonoBehaviour
{
    [field: SerializeField] public AudioCollection DiedSound { get; private set; }
    [field: SerializeField] public AudioCollection HurtSound { get; private set; }

    private Character m_Character;

    private void Awake() => m_Character = GetComponent<Character>();

    private void OnEnable()
    {
        if (!m_Character.IsAlive)
            return;

        m_Character.OnHit += OnHit;
        m_Character.OnDied += OnDeath;
    }

    private void OnDisable()
    {
        if (!m_Character.IsAlive)
            return;

        m_Character.OnHit -= OnHit;
        m_Character.OnDied -= OnDeath;
    }

    private void OnHit(float health)
    {
        // add controling volume by health. volume = health value
        AudioManager.CreateAudioInstance(HurtSound, transform.position);
    }

    private void OnDeath()
    {
        AudioManager.CreateAudioInstance(DiedSound, transform.position);

        m_Character.OnHit -= OnHit;
        m_Character.OnDied -= OnDeath;
    }
}
