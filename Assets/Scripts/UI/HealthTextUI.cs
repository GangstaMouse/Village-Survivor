using TMPro;
using UnityEngine;

public class HealthTextUI : MonoBehaviour
{
    [SerializeField] TMP_Text m_HealthText;

    private void Start()
    {
        if (Player.Instance == null)
            return;

        Player.Instance.OnHitLocal += (s, t) => SetHealthText(((Character)t).Health);
        SetHealthText(Player.Instance.Health);
    }

    private void SetHealthText(float value) => m_HealthText.SetText($"Health: {value:F1}");
}
