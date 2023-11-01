using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

class PlayerInputHandler : InputHandler<PlayerInputHandlerInstance>
{
    [SerializeField] private InputActionReference m_AttackInputAction;
    [SerializeField] private InputActionReference m_MouseInputAction;
    [SerializeField] private InputActionReference m_MovementInputAction;
    [SerializeField] private InputActionReference m_DashInputAction;

    protected override void Initialize()
    {
        InputHandlerInstance = new PlayerInputHandlerInstance(transform, GetComponentInChildren<Camera>(),
            m_AttackInputAction, m_MouseInputAction, m_MovementInputAction, m_DashInputAction);
    }
}

class PlayerInputHandlerInstance : InputHandlerInstance
{
    // remove
    private readonly Transform m_CharacterTransform;
    private readonly Camera m_Camera;
    // ---
    public readonly InputActionReference AttackInputAction;
    public readonly InputActionReference MouseInputAction;
    public readonly InputActionReference MovementInputAction;
    public readonly InputActionReference DashInputAction;

    public override float2 LookingDirection
    {
        get
        {
            // remake, use just screen resolution, calculate center, then find direction from that center
            float2 m_ScreenMousePosition = MouseInputAction.action.ReadValue<Vector2>();
            Vector3 m_WorldMousePosition = m_Camera.ScreenToWorldPoint(new(m_ScreenMousePosition.x, m_ScreenMousePosition.y, 1)) - m_CharacterTransform.position;
            return math.normalizesafe(new float2(m_WorldMousePosition.x, m_WorldMousePosition.y));
        }
    }
    public override float2 MovementInput => MovementInputAction.action.ReadValue<Vector2>();

    public PlayerInputHandlerInstance(in Transform transform, in Camera camera, in InputActionReference attackActionReference,
         in InputActionReference mouseInputAction, in InputActionReference movementInputAction, in InputActionReference dashInputAction)
    {
        m_CharacterTransform = transform;
        m_Camera = camera;
        AttackInputAction = attackActionReference;
        MouseInputAction = mouseInputAction;
        MovementInputAction = movementInputAction;
        DashInputAction = dashInputAction;

        AttackInputAction.action.performed += (context) => RaiseOnAttackInitiated();
        AttackInputAction.action.canceled += (context) => RaiseOnAttackReleased();
        DashInputAction.action.performed += (context) => Dash();
    }
}
