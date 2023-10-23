using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    [SerializeField] private InputActionReference m_MovementInputAction;
    [SerializeField] private InputActionReference m_DashInputAction;
    [SerializeField] private float BaseDashRange = 2.0f;
    private Stats m_Stats;
    private float m_CoolDownTimer = 0.0f;

    private void Start() => m_Stats = GetComponent<Character>().Stats;
    private void OnEnable() => m_DashInputAction.action.performed += (e) => ActivateAbility();
    private void OnDisable() => m_DashInputAction.action.performed -= (e) => ActivateAbility();
    private void ActivateAbility()
    {
        Debug.LogWarning("Dash!");
        Vector2 movementInput = m_MovementInputAction.action.ReadValue<Vector2>();

        if (m_CoolDownTimer > 0 || math.length(movementInput) <= 0.4f)
            return;

        Vector2 dashVector = movementInput * BaseDashRange * m_Stats.GetStat("Dash distance").Value;
        transform.Translate(dashVector);
        m_CoolDownTimer = m_Stats.GetStat("Dash cooldown").Value;
    }

    private void FixedUpdate()
    {
        m_CoolDownTimer = math.max(m_CoolDownTimer - Time.fixedDeltaTime, 0.0f);
    }
}
