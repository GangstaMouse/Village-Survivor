using UnityEngine;

[CreateAssetMenu(fileName = "New Game Mode Data", menuName = "Game Mode Data")]
public class GameModeData : ScriptableObject
{
    [field: SerializeField] public SpawnerParameters SpawnerParameters;
    [field: SerializeField] public MobsContainer MobsContainer;
    [field: SerializeField] public float StagesLenght = 30;
}
