using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

class PlayerInputHandler : InputHandler
{
    [SerializeField] private InputActionReference m_AttackInputAction;
    [SerializeField] private InputActionReference m_MouseInputAction;
    [SerializeField] private InputActionReference m_MovementInputAction;
    [SerializeField] private InputActionReference m_DashInputAction;

    // remove
    private Transform m_CharacterTransform;
    private Camera m_Camera;
    // ---

    private void FixedUpdate()
    {
        MovementInput = m_MovementInputAction.action.ReadValue<Vector2>();

        float2 m_ScreenMousePosition = m_MouseInputAction.action.ReadValue<Vector2>();
        Vector3 m_WorldMousePosition = m_Camera.ScreenToWorldPoint(new(m_ScreenMousePosition.x, m_ScreenMousePosition.y, 1)) - m_CharacterTransform.position;
        LookingInput = math.normalizesafe(new float2(m_WorldMousePosition.x, m_WorldMousePosition.y));
    }

    protected override void Initialize()
    {
        m_CharacterTransform = transform;
        m_Camera = GetComponentInChildren<Camera>();

        m_AttackInputAction.action.performed += (context) => OnAttackInitiated?.Invoke();
        m_AttackInputAction.action.canceled += (context) => OnAttackReleased?.Invoke();
        m_DashInputAction.action.performed += (context) => OnDash?.Invoke();
    }
}
