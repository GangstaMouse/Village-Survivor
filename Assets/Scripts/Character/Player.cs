using System;
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
    [SerializeField] InputActionReference m_DodgeAction;

    public static event Action<int, float> OnExperienceChanged;

    public int Level = 0;
    public float Experience = 0;
    public ModificableValue ExperienceMult;

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

    private void OnEnable()
    {
        m_AttackAction.action.performed += (e) => Attack();
        m_DodgeAction.action.performed += (e) => Dodge();
    }
    private void OnDisable()
    {
        m_AttackAction.action.performed -= (e) => Attack();
        m_DodgeAction.action.performed -= (e) => Dodge();
    }

    private void Dodge()
    {
        if (math.length(MovementInput) == 0 || dodgecooltimer != 0)
            return;

        Vector2 dodgeVector = MovementInput * dodgeRange;
        dodgecooltimer = dodgeCool;
        transform.Translate(dodgeVector);
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        /* dam.AddModificator(new Modificator()
        {
            Value = 10
        }); */

        Instance = this;

        m_Camera = GetComponentInChildren<Camera>();

        // remake
        OnHitLocal += (damager, target) =>
        {
            if (damager == this && target.IsAlive == false)
                AddExperience();
        };
    }

    private void AddExperience()
    {
        Experience += ExperienceMult.Value;
        
        if (Experience >= 10)
        {
            Level++;
            Experience = 0;
        }

        OnExperienceChanged?.Invoke(Level, Experience);
    }

    protected override void OnFixedUpdate()
    {
        dodgecooltimer = math.max(dodgecooltimer - Time.fixedDeltaTime, 0);
    }
}
