using Frameworks.Navigation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class Player : Character
{
    // Input
    [Header("Input Actions")]
    [SerializeField] InputActionReference m_MouseAction;
    [SerializeField] InputActionReference m_MovementAction;
    [SerializeField] InputActionReference m_AttackAction;

    // Internal variables
    public static Player Instance { get; private set; }

    public override Vector2 LookDirection { get
    {
        m_ScreenMousePosition = m_MouseAction.action.ReadValue<Vector2>();
        Vector3 m_WorldMousePosition = m_Camera.ScreenToWorldPoint(new(m_ScreenMousePosition.x, m_ScreenMousePosition.y, 1)) - transform.position;
        return math.normalizesafe(new float2(m_WorldMousePosition.x, m_WorldMousePosition.y));
    }}
    public override Vector2 MovementInput => m_MovementAction.action.ReadValue<Vector2>();

    private Camera m_Camera;
    private float2 m_ScreenMousePosition;
    public float dodgeRange = 1;
    public float dodgeCool = 5;
    public float dodgecooltimer;

    private WeaponContainer weaponContainer;

    private void OnEnable()
    {
        m_AttackAction.action.performed += (e) => weaponContainer.InitiateAttack();
        m_AttackAction.action.canceled += (e) => weaponContainer.ReleaseAttack();
    }
    private void OnDisable()
    {
        m_AttackAction.action.performed -= (e) => weaponContainer.InitiateAttack();
        m_AttackAction.action.canceled -= (e) => weaponContainer.ReleaseAttack();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            return;
        }

        Navigation2D.target = transform;

        Instance = this;

        m_Camera = GetComponentInChildren<Camera>();
        weaponContainer = GetComponent<WeaponContainer>();
    }

    protected override void OnFixedUpdate()
    {
        // Navigation2DBurst.CalculateFlowMap(Frameworks.Navigation.Utils.GetClosestVoxel(transform.position, Navigation2D.m_GeneratedMap), Navigation2D.m_GeneratedMap, out Navigation2D.flowNav);
        // throw new NotImplementedException();
    }
}
