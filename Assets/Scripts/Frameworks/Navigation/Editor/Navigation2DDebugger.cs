#if UNITY_EDITOR
using System;
using Unity.Mathematics;
using UnityEngine;
using static Frameworks.Navigation.Utils;

namespace Frameworks.Navigation
{
    class Navigation2DDebugger : MonoBehaviour
    {
        [SerializeField] private bool m_ShowPathFlow;
        private NavigationData navigationData;

        NavigationGrid2D m_GeneratedMap;
        [SerializeField] private Vector2 m_DebugPoint;
        private float MaxCost;

        private void OnDrawGizmosSelected()
        {
            m_GeneratedMap = Navigation2D.m_GeneratedMap;
            navigationData = Navigation2D.flowNav;

            if (m_GeneratedMap.GridSize == Vector2Int.zero)
                return;

            // Calculate max cost, except infinity
            bool flowDataExists = navigationData.ReachCost != null;

            if (flowDataExists)
            {
                MaxCost = 0;

                for (int i = 0; i < m_GeneratedMap.GridSize.x * m_GeneratedMap.GridSize.y; i++)
                {
                    float cost = navigationData.ReachCost[i];

                    if (cost > MaxCost && cost != math.INFINITY)
                        MaxCost = cost;
                }
            }

            // Draw Cells
            for (int y = 0; y < m_GeneratedMap.GridSize.y; y++)
                for (int x = 0; x < m_GeneratedMap.GridSize.x; x++)
                {
                    Vector2 worldVoxelPoint = LocalToWorldVoxel(new(x, y), m_GeneratedMap);

                    if (flowDataExists)
                        Gizmos.color = Color.Lerp(Color.white, Color.black, navigationData.ReachCost[y * m_GeneratedMap.GridSize.x + x] / MaxCost); // test
                    else
                        Gizmos.color = (Convert.ToBoolean(m_GeneratedMap.ObstaclesData[y * m_GeneratedMap.GridSize.x + x]) == false) ? Color.grey : Color.black;

                    Gizmos.DrawCube(worldVoxelPoint, Vector3.one * 0.9f);
                }

            // Draw path flow
            if (m_ShowPathFlow && flowDataExists)
            {
                Gizmos.color = Color.black;

                for (int i = 0; i < navigationData.ReachedFrom.Length; i++)
                {
                    if (navigationData.ReachedFrom[i] == -1)
                        continue;

                    Vector2 worldVoxelA = LocalToWorldVoxel(GetLocalVoxelFromIndex(i, m_GeneratedMap.GridSize), m_GeneratedMap);
                    Vector2 worldVoxelB = LocalToWorldVoxel(GetLocalVoxelFromIndex(navigationData.ReachedFrom[i], m_GeneratedMap.GridSize), m_GeneratedMap);
                    Gizmos.DrawLine(worldVoxelA, worldVoxelB);
                }
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(LocalToWorldVoxel(GetLocalVoxelFromIndex(
                GetClosestVoxel(transform.TransformPoint(m_DebugPoint), m_GeneratedMap), m_GeneratedMap.GridSize), m_GeneratedMap), 0.5f);

            Gizmos.color = Color.white;
            // Gizmos.DrawLine(transform.position, transform.position + (transform.forward * ))
        }
    }
}
#endif
