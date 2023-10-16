using TMPro;
using UnityEngine;

public class ScoreTextUI : MonoBehaviour
{
    [SerializeField] TMP_Text m_ScoreText;

    private void OnEnable()
    {
        GameModeManager.OnScoresChanged += RefreshText;
        RefreshText(GameModeManager.Scores);
    }

    private void RefreshText(float value) => m_ScoreText.SetText($"Score: {value:F0}");
}
