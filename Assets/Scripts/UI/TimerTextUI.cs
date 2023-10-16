using TMPro;
using UnityEngine;

public class TimerTextUI : MonoBehaviour
{
    [SerializeField] TMP_Text m_TimerText;

    private void OnEnable()
    {
        GameModeManager.OnTimerUpdated += RefreshText;
        RefreshText(GameModeManager.Timer);
    }

    private void RefreshText(float value) => m_TimerText.SetText($"Time: {value:F0}");
}
