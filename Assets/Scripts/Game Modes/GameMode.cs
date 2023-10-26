using System;
using System.Collections.Generic;
using UnityEngine;

public class GameMode
{
    public readonly GameModeData m_GameModeData;
    public int CurrentStage { get; private set; } = 1;
    public float Timer { get; private set; } = 0.0f;
    public event Action<int> OnStageChanged;

    public GameMode(in GameModeData gameModeData)
    {
        m_GameModeData = gameModeData;
    }

    public void Initialize()
    {
        MobSpawner.Init();
        MobSpawner.Enable(m_GameModeData.SpawnerParameters, GetMobsAtStage(CurrentStage));
    }

    public void Run()
    {
        Timer += Time.fixedDeltaTime;

        float nextStageTime = CurrentStage * m_GameModeData.StagesLenght;

        if (Timer >= nextStageTime)
        {
            CurrentStage++;

            MobSpawner.Enable(m_GameModeData.SpawnerParameters, GetMobsAtStage(CurrentStage));
            OnStageChanged?.Invoke(CurrentStage);
            Debug.LogWarning("New Stage!");
        }
    }

    // public abstract void End();

    private List<GameObject> GetMobsAtStage(in int stage)
    {
        List<GameObject> mobsToSpawn = new();

        foreach (var mobs in m_GameModeData.MobsContainer.Stages)
        {
            if (mobs.AppearsInTheStages.Contains(stage))
            {
                mobsToSpawn.Add(mobs.MobPrefab);
            }
        }

        return mobsToSpawn;
    }
}
