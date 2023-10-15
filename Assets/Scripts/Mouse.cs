using UnityEngine;
using UnityEngine.InputSystem;

public class Mouse : MonoBehaviour
{
    [SerializeField] private InputActionReference m_MouseAction;

    private Vector2 m_ScreenMousePosition;
    private Camera m_Camera;

    private void Awake()
    {
        m_Camera = GetComponentInParent<Camera>();
    }

    private void FixedUpdate()
    {
        m_ScreenMousePosition = m_MouseAction.action.ReadValue<Vector2>();
        Vector3 m_WorldMousePosition = m_Camera.ScreenToWorldPoint(new Vector3(m_ScreenMousePosition.x, m_ScreenMousePosition.y, 1));
        transform.position = m_WorldMousePosition;
    }
}
