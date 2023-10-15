using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TMP_Text health;

    private void OnEnable()
    {
        GameModeManager.OnTimeUpdated += Te;
        Te(GameModeManager.time);
    }

    private void Te(float value) => health.SetText($"Time: {value:F0}");
}
