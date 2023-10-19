using UnityEngine;

[CreateAssetMenu(fileName = "New Game Mode Data", menuName = "Game Mode Data")]
public class GameModeData : ScriptableObject
{
    public SpawnerParameters SpawnerParameters => m_SpawnerParameters;
    public MobsContainer MobsContainer => m_MobsContainer;
    public float StagesLenght => m_StagesLenght;

    [SerializeField] private SpawnerParameters m_SpawnerParameters;
    [SerializeField] private MobsContainer m_MobsContainer;
    [SerializeField] private float m_StagesLenght = 30;
}
