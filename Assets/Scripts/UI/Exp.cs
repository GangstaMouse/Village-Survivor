using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Exp : MonoBehaviour
{
    [SerializeField] TMP_Text m_Level;
    [SerializeField] TMP_Text m_Experience;

    private void OnEnable() => Player.OnExperienceChanged += Te;
    private void OnDisable() => Player.OnExperienceChanged -= Te;

    private void Te(int level, float experience)
    {
        m_Level.SetText($"Level: {level:F0}");
        m_Experience.SetText($"Experience: {experience:F0}");
    }
}
