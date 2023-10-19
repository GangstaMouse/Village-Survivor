using UnityEngine;

class WeaponDebug : MonoBehaviour
{
    [SerializeField] private WeaponShootModule m_ShootModule;

    private void OnDrawGizmos()
    {
        if (m_ShootModule == null)
            return;

        m_ShootModule.Debug(transform.position);
    }
}
