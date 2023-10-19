using System;
using Unity.Mathematics;
using UnityEngine;
using Framework.Navigation;
using static Framework.Navigation.Utils;
using System.Collections.Generic;

public class Navigation2DDebugger : MonoBehaviour
{
    private NavigationGrid2D m_GeneratedMap => Navigation2D.m_GeneratedMap;
    [SerializeField] private bool m_ShowCosts;
    [SerializeField] private bool m_ShowPathFlow;
    [SerializeField] private bool m_ShowPath;
    [SerializeField] Vector2 m_DebugStartPoint = Vector2.zero;
    [SerializeField] Vector2 m_DebugEndPoint = Vector2.one * 4;
    private float[] m_PathCosts;
    private int[] m_PathFlow;
    private Vector2Int[] m_Path;

    private float m_MaxReachCost = 0;

    private void FindPath()
    {
        int fromVoxelIndex = GetClosestVoxel(m_DebugStartPoint, m_GeneratedMap.GridSize, transform);
        int toVoxelIndex = GetClosestVoxel(m_DebugEndPoint, m_GeneratedMap.GridSize, transform);
        
        Navigation2D.FindPathTo(fromVoxelIndex, toVoxelIndex, out Navigation2D.NavigationData navigationData, out int lastReachedVoxelIndex);

        m_PathCosts = navigationData.ReachCost;
        m_PathFlow = navigationData.ReachedFrom;

        List<Vector2Int> path = new();
        int currentVoxelIndex = lastReachedVoxelIndex;

        while(currentVoxelIndex != fromVoxelIndex)
        {
            Vector2Int voxelLocalPosition = GetLocalVoxelFromIndex(currentVoxelIndex, m_GeneratedMap.GridSize);
            path.Add(voxelLocalPosition);
            currentVoxelIndex = navigationData.ReachedFrom[currentVoxelIndex];
        }

        m_Path = path.ToArray();
    }

    private void OnValidate()
    {
        if (TryGetComponent(out Navigation2D navigation))
            navigation.BuildNavigation();

        FindPath();

        // Calculate max reach cost for debug drawings
        if (m_ShowCosts)
            for (int i = 0; i < m_PathCosts.Length; i++)
            {
                float cost = m_PathCosts[i];

                if (cost < math.INFINITY && cost > m_MaxReachCost)
                    m_MaxReachCost = cost;
            }
    }

    private void OnDrawGizmos()
    {
        if (m_GeneratedMap.GridSize == Vector2Int.zero)
            return;

        for (int y = 0; y < m_GeneratedMap.GridSize.y; y++)
            for (int x = 0; x < m_GeneratedMap.GridSize.x; x++)
            {
                Vector2 worldVoxelPoint = LocalToWorldVoxel(x, y, transform);
                Gizmos.color = Convert.ToBoolean(m_GeneratedMap.ObstaclesData[y * m_GeneratedMap.GridSize.x + x]) ? Color.black : Color.grey;

                if (m_ShowCosts && m_PathCosts.Length > 0)
                    Gizmos.color = Color.Lerp(Color.white, Color.black, m_PathCosts[y * m_GeneratedMap.GridSize.x + x] / m_MaxReachCost);

                Gizmos.DrawCube(worldVoxelPoint, Vector3.one * 0.9f);
            }

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(LocalToWorldVoxel(GetLocalVoxelFromIndex(
            GetClosestVoxel(m_DebugStartPoint, m_GeneratedMap.GridSize, transform), m_GeneratedMap.GridSize), transform), 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(LocalToWorldVoxel(GetLocalVoxelFromIndex(
            GetClosestVoxel(m_DebugEndPoint, m_GeneratedMap.GridSize, transform), m_GeneratedMap.GridSize), transform), 0.5f);

        Gizmos.color = Color.yellow;

        if (m_ShowPathFlow)
        {
            for (int i = 0; i < m_PathFlow.Length; i++)
            {
                if (m_PathFlow[i] == -1)
                    continue;
                
                Vector2 worldVoxelA = LocalToWorldVoxel(GetLocalVoxelFromIndex(i, m_GeneratedMap.GridSize), transform);
                Vector2 worldVoxelB = LocalToWorldVoxel(GetLocalVoxelFromIndex(m_PathFlow[i], m_GeneratedMap.GridSize), transform);
                Gizmos.DrawLine(worldVoxelA, worldVoxelB);
            }
        }

        if (m_ShowPath)
        {
            Vector2 worldVoxelA = LocalToWorldVoxel(m_Path[0], transform);
            
            for (int i = 1; i < m_Path.Length; i++)
            {
                Vector2 worldVoxelB = LocalToWorldVoxel(m_Path[i], transform);
                Gizmos.DrawLine(worldVoxelA, worldVoxelB);
                worldVoxelA = worldVoxelB;
            }
        }
    }
}
