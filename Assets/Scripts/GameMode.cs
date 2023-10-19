using System.Collections.Generic;
using UnityEngine;

public class GameMode
{
    public GameMode(SpawnerParameters spawnerParameters, MobsContainer mobsContainer, float stageLenght)
    {
        m_SpawnerParameters = spawnerParameters;
        m_MobsContainer = mobsContainer;
        m_StagesLenght = stageLenght;
    }

    private SpawnerParameters m_SpawnerParameters;
    private MobsContainer m_MobsContainer;

    private float m_StagesLenght = 30;
    private int m_CurrentStage = 1;
    private float Timer = 0.0f;

    public void Init()
    {
        MobSpawner.Init();
        MobSpawner.Enable(m_SpawnerParameters, GetMobsAtStage(m_CurrentStage));
    }

    public void Run()
    {
        Timer += Time.fixedDeltaTime;

        float nextStageTime = m_CurrentStage * m_StagesLenght;

        if (Timer >= nextStageTime)
        {
            m_CurrentStage++;

            List<GameObject> stageMobs = GetMobsAtStage(m_CurrentStage);
            MobSpawner.Enable(m_SpawnerParameters, stageMobs);
            Debug.LogWarning("New Stage!");
        }
    }

    // public abstract void End();

    private List<GameObject> GetMobsAtStage(int stage)
    {
        Debug.LogError(stage);
        List<GameObject> mobsToSpawn = new();

        foreach (var mobs in m_MobsContainer.Stages)
        {
            Debug.LogError(mobs.AppearsInTheStages);
            if (mobs.AppearsInTheStages.Contains(stage))
            {
                mobsToSpawn.Add(mobs.MobPrefab);
            }
        }

        Debug.LogError(mobsToSpawn.Count);

        return mobsToSpawn;
    }
}
