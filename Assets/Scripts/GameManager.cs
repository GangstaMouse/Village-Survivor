using System;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public GameModeManager Instance { get; private set; }
    public GameMode GameModeInstance { get; private set; }
    public GameMode gameMode;
    public static event Action<float> OnScoreChanged;
    public static float scores;
    public static MobsCont cont;
    public static int stage;
    public static float time;
    public static event Action<float> OnTimeUpdated;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        cont = gameMode.mobsCont;
        gameMode.PreInit(cont);

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        Character.OnDiedGlobal += addSc;
    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        OnTimeUpdated?.Invoke(time);
    }

    private void addSc()
    {
        scores++;
        OnScoreChanged?.Invoke(scores);
    }


}