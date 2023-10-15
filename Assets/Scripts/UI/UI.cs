using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] TMP_Text health;

    private void OnEnable()
    {
        if (Player.Instance == null)
            return;

        Player.Instance.OnDamageTakenss += SetHealthText;
        SetHealthText(Player.Instance.Health);
    }

    private void SetHealthText(float value)
    {
        health.SetText($"Health: {value:F1}");
    }
}
