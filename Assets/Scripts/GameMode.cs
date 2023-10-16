using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Mode", menuName = "Game Mode")]
public class GameMode : ScriptableObject
{
    [SerializeField] SpawnerParameters m_SpawnerParameters;
    [SerializeField] private MobsContainer m_MobsContainer;

    protected float m_StagesLenght = 30;
    protected int m_CurrentStage = 1;
    protected float Timer = 0.0f;

    public virtual void Init()
    {
        MobSpawner.Init();
        MobSpawner.Enable(m_SpawnerParameters, GetMobsAtStage(m_CurrentStage));
    }

    public virtual void Run()
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
        List<GameObject> mobsToSpawn = new();

        foreach (var mobs in m_MobsContainer.Stages)
        {
            if (mobs.AppearsInTheStages.Contains(stage))
            {
                mobsToSpawn.Add(mobs.MobPrefab);
                break;
            }
        }

        return mobsToSpawn;
    }
}
