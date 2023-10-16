using TMPro;
using UnityEngine;

public class LevelExperienceTextUI : MonoBehaviour
{
    [SerializeField] TMP_Text m_LevelText;
    [SerializeField] TMP_Text m_ExperienceText;

    private void OnEnable() => Player.OnExperienceChanged += RefreshTexts;
    private void OnDisable() => Player.OnExperienceChanged -= RefreshTexts;

    private void RefreshTexts(int level, float experience)
    {
        m_LevelText.SetText($"Level: {level:F0}");
        m_ExperienceText.SetText($"Experience: {experience:F0}");
    }
}
