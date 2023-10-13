using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class Player : Character
{
    // Input
    [SerializeField] InputActionReference m_MouseAction;
    [SerializeField] InputActionReference m_MovementAction;
    [SerializeField] InputActionReference m_AttackAction;
    private float2 m_ScreenMousePosition;
    private Camera m_Camera;

    // Internal variables
    public static Player Instance { get; private set; }

    private void OnEnable() => m_AttackAction.action.performed += (e) => Attack();
    private void OnDisable() => m_AttackAction.action.performed -= (e) => Attack();

    public override void OnDamageTaken(float value)
    {
        // throw new System.NotImplementedException();
    }

    protected override Vector2 Movement()
    {
        return m_MovementAction.action.ReadValue<Vector2>();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;

        m_Camera = GetComponentInChildren<Camera>();
    }

    protected override Vector2 Looking()
    {
        m_ScreenMousePosition = m_MouseAction.action.ReadValue<Vector2>();
        Vector3 m_WorldMousePosition = m_Camera.ScreenToWorldPoint(new(m_ScreenMousePosition.x, m_ScreenMousePosition.y, 1)) - transform.position;
        return math.normalizesafe(new float2(m_WorldMousePosition.x, m_WorldMousePosition.y));
    }
}
