using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mouse : MonoBehaviour, ILookInput
{
    [SerializeField] private InputActionReference m_MouseAction;

    private Vector2 m_ScreenMousePosition;
    private Camera m_Camera;
    private Character character;

    // private void OnEnable() => character.OverrideLookInput(this);
    // private void OnDisable() => character.OverrideLookInput(null);

    private void Awake()
    {
        character = GetComponentInParent<Character>();
        m_Camera = GetComponentInParent<Camera>();
    }

    private void FixedUpdate()
    {
        m_ScreenMousePosition = m_MouseAction.action.ReadValue<Vector2>();
        Vector3 m_WorldMousePosition = m_Camera.ScreenToWorldPoint(new Vector3(m_ScreenMousePosition.x, m_ScreenMousePosition.y, 1));
        transform.position = m_WorldMousePosition;
    }

    public Vector2 GetInput()
    {
        m_ScreenMousePosition = m_MouseAction.action.ReadValue<Vector2>();
        Vector3 m_WorldMousePosition = m_Camera.ScreenToWorldPoint(new(m_ScreenMousePosition.x, m_ScreenMousePosition.y, 1));
        return new(m_WorldMousePosition.x, m_WorldMousePosition.y);
    }
}
