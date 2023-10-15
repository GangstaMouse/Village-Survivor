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
    public ModificableValue Damage;

    public int Level = 0;
    public float Experience = 0;
    public ModificableValue ExperienceMult;

    // Internal variables
    public static Player Instance { get; private set; }
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
        if (math.length(m_MovementInput) == 0 || dodgecooltimer != 0)
            return;

        Vector2 dodgeVector = m_MovementInput * dodgeRange;
        dodgecooltimer = dodgeCool;
        transform.Translate(new Vector3(dodgeVector.x, dodgeVector.y, 0));
    }

    public override void OnDamageTaken(float value) { }

    protected override Vector2 Movement() => m_MovementAction.action.ReadValue<Vector2>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;

        m_Camera = GetComponentInChildren<Camera>();

        OnHit += (damager, target) =>
        {
            if (damager == (IDamager)this && target.IsAlive == false)
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

    protected override Vector2 Looking()
    {
        m_ScreenMousePosition = m_MouseAction.action.ReadValue<Vector2>();
        Vector3 m_WorldMousePosition = m_Camera.ScreenToWorldPoint(new(m_ScreenMousePosition.x, m_ScreenMousePosition.y, 1)) - transform.position;
        return math.normalizesafe(new float2(m_WorldMousePosition.x, m_WorldMousePosition.y));
    }

    protected override void OnFixedUpdate()
    {
        dodgecooltimer = math.max(dodgecooltimer - Time.fixedDeltaTime, 0);
    }

    protected override void OnKilled()
    {
        
    }
}
