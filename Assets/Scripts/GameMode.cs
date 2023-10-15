using UnityEngine;

public abstract class GameMode : ScriptableObject
{
    [SerializeField] SpawnerParameters m_SpawnerParameters;
    public MobsCont mobsCont;

    public void PreInit(MobsCont mobsCont)
    {
        MobSpawner.Init();
        MobSpawner.Enable(m_SpawnerParameters, mobsCont.Stages[0].mobs);
        Init();
        // MobSpawner.Disable();
        // MobSpawner.Enable(mobsCont.Stages[0].mobs);
    }

    public abstract void Init();
    public abstract void Run();
    public abstract void End();
}
