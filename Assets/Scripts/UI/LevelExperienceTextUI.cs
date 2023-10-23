using TMPro;
using UnityEngine;

public class LevelExperienceTextUI : MonoBehaviour
{
    [SerializeField] TMP_Text m_LevelText;
    [SerializeField] TMP_Text m_ExperienceText;

    private void UpdateLevelText(int value)
    {
        m_LevelText.SetText($"Level: {value:F0}");
    }

    private void UpdateExperienceText(int value)
    {
        m_ExperienceText.SetText($"Experience: {value:F0}");
    }

    private void OnEnable()
    {
        LevelComponent.OnLevelChanged += UpdateLevelText;
        LevelComponent.OnExperienceChanged += UpdateExperienceText;
    }

    private void OnDisable()
    {
        LevelComponent.OnLevelChanged -= UpdateLevelText;
        LevelComponent.OnExperienceChanged += UpdateExperienceText;
    }
}
