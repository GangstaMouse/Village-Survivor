using System;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public GameModeManager Instance { get; private set; }

    [SerializeField] private GameModeData m_GameModeData;
    private GameMode m_GameMode;

    public static float Timer { get; private set; }
    public static int Stage { get; private set; }
    public static float Scores { get; private set; }

    public static event Action<float> OnTimerUpdated;
    public static event Action<float> OnStageChanged;
    public static event Action<float> OnScoresChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        IDamageble.OnHit += DoesEnemyDied;

        m_GameMode = new(m_GameModeData.SpawnerParameters, m_GameModeData.MobsContainer, m_GameModeData.StagesLenght);
        m_GameMode.Init();
    }

    private void DoesEnemyDied(IDamageSource damageSource, IDamageble damageble)
    {
        if (damageSource is Player && ((Character)damageble).Health <= 0)
            AddScores();
    }

    private void FixedUpdate()
    {
        Timer += Time.fixedDeltaTime;
        OnTimerUpdated?.Invoke(Timer);

        m_GameMode.Run();
    }

    private void AddScores()
    {
        Scores++;
        OnScoresChanged?.Invoke(Scores);
    }
}