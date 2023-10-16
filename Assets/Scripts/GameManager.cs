using System;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public GameModeManager Instance { get; private set; }

    [SerializeField] private GameMode m_GameModes;

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
        m_GameModes.Init();
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

        m_GameModes.Run();
    }

    private void AddScores()
    {
        Scores++;
        OnScoresChanged?.Invoke(Scores);
    }
}