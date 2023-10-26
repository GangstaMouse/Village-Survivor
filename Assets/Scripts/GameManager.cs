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

        ImpactSystem.OnDiedEvent += DoesEnemyDied;

        m_GameMode = new(m_GameModeData);
        m_GameMode.Initialize();
    }

    private void DoesEnemyDied(IDamageble damageble)
    {
        if (damageble != (IDamageble)Player.Instance.Character)
            AddScores();
    }

    // private void ResetOnRestart? or just game mode for plays??

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