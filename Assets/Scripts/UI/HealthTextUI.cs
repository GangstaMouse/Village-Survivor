using TMPro;
using UnityEngine;

public class HealthTextUI : MonoBehaviour
{
    [SerializeField] TMP_Text m_HealthText;

    private void OnEnable()
    {
        if (Player.Instance == null)
            return;

        Player.Instance.OnHitLocal += (s, t) => SetHealthText(((Character)s).Health);
        SetHealthText(Player.Instance.Health);
    }

    private void SetHealthText(float value)
    {
        m_HealthText.SetText($"Health: {value:F1}");
    }
}
