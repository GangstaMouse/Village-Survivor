using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

class CameraController : MonoBehaviour
{
    [SerializeField] private float m_DisplacementValue = 1.4f;
    [SerializeField] private Transform m_CursorTransform;
    [SerializeField] private InputActionReference m_MouseInputAction;
    private Vector2 m_MouseScreenPoint;
    private Vector3 m_MouseWorldPoint;
    private Transform m_CameraTarget;
    private Vector3 m_CameraOffset;
    private Camera m_Camera;

    private void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_CameraTarget = transform.parent;
        m_CameraOffset = transform.localPosition;
        transform.SetParent(null);
    }

    private void FixedUpdate()
    {
        m_MouseScreenPoint = m_MouseInputAction.action.ReadValue<Vector2>();

        Vector2 screenRes = new(Screen.width, Screen.height);
        float minRes = math.min(Screen.width, Screen.height);
        Vector2 rawDisplacement = (m_MouseScreenPoint - (screenRes / 2.0f)) / minRes * 2.0f;

        Vector2 displacement = (math.length(rawDisplacement) > 1.0f) ? math.normalize(rawDisplacement) : rawDisplacement;
        displacement *= m_DisplacementValue;
        Vector3 cameraDisplacement3D = new(displacement.x, displacement.y, 0.0f);

        transform.position = m_CameraTarget.position + m_CameraOffset + cameraDisplacement3D;
        m_MouseWorldPoint = m_Camera.ScreenToWorldPoint(new Vector3(m_MouseScreenPoint.x, m_MouseScreenPoint.y, 1));
        m_CursorTransform.position = m_MouseWorldPoint;
    }
}
