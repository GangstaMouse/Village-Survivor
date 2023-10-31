using Unity.Mathematics;
using UnityEngine;

public class Dash : MonoBehaviour, IInputReceiver
{
    [SerializeField] private float BaseDashRange = 2.0f;
    private Stats m_Stats;
    private float m_CoolDownTimer = 0.0f;

    InputHandlerInstance IInputReceiver.InputHandler { get => InputHandler; set => InputHandler = value; }
    InputHandlerInstance InputHandler = new();

    private void Start() => m_Stats = GetComponent<Character>().Stats;
    private void ActivateAbility()
    {
        Debug.LogWarning("Dash!");
        Vector2 movementInput = InputHandler.MovementInput;

        if (m_CoolDownTimer > 0 || math.length(movementInput) <= 0.4f)
            return;

        Vector2 dashVector = movementInput * BaseDashRange * m_Stats.GetStat("Dash distance").Value;
        transform.Translate(dashVector, Space.World);
        m_CoolDownTimer = m_Stats.GetStat("Dash cooldown").Value;
    }

    private void FixedUpdate()
    {
        m_CoolDownTimer = math.max(m_CoolDownTimer - Time.fixedDeltaTime, 0.0f);
    }

    void IInputReceiver.OnInputHandlerChanged(in InputHandlerInstance inputHandler)
    {
        inputHandler.OnDash += ActivateAbility;
    }
}
