using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform m_Target;
    private Vector3 m_Offset;
    private void Start()
    {
        m_Target = transform.parent;
        m_Offset = transform.localPosition;
        transform.SetParent(null);
    }

    private void LateUpdate()
    {
        transform.position = m_Target.position + m_Offset;
    }
}
