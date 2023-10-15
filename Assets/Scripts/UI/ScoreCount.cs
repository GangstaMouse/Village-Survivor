using TMPro;
using UnityEngine;

public class ScoreCount : MonoBehaviour
{
    [SerializeField] TMP_Text health;

    private void OnEnable()
    {
        GameModeManager.OnScoreChanged += Te;
        Te(GameModeManager.scores);
    }

    private void Te(float value) => health.SetText($"Score: {value:F0}");
}
