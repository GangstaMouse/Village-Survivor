using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class Navigation2D : MonoBehaviour
{
    [SerializeField] private Vector2Int Resolution = new(16, 16);
    [SerializeField] private LayerMask layerMask;
    private bool[,] m_GeneratedMap;
    private float[,] m_Costs;
    [SerializeField] private bool m_ShowCosts;
    private int[,] connects;
    [SerializeField] private bool m_ShowPathMap;

    [SerializeField] Vector2 m_DebugStartPoint = Vector2.zero;
    [SerializeField] Vector2 m_DebugEndPoint = Vector2.one * 4;

    public static class PathFinderRules
    {
        public static void PathTo(in int fromIndex, in int toIndex, in bool[,] map, out int[,] conns, out float[,] costs)
        {
            Vector2Int mapSize = new(map.GetLength(0), map.GetLength(1));
            int[,] pathMap = new int[mapSize.x, mapSize.y];
            float[,] costsMap = new float[mapSize.x, mapSize.y];

            Debug.LogWarning(toIndex);

            int currentNodeIndex = fromIndex;
            int targetNode = toIndex;

            List<int> passedNodes = new();
            List<int> toSearchNodes = new() { currentNodeIndex };

            int iterations = 0;
            bool found = false;

            while (toSearchNodes.Count > 0 && found == false)
            {
                if (iterations > (mapSize.x * mapSize.y))
                    throw new Exception("Infinite loop in the navigation system");

                iterations++;

                currentNodeIndex = toSearchNodes[0];
                Vector2Int currentNodePosition = GetVoxel2D(currentNodeIndex, mapSize);

                toSearchNodes.Remove(currentNodeIndex);
                passedNodes.Add(currentNodeIndex);

                int[] closestNodes = GetClosestNodes(currentNodePosition, mapSize);

                if (currentNodeIndex == targetNode)
                {
                    // Debug.Log("Path was found!");
                    // conns = pathMap;
                    costs = costsMap;
                    found = true;
                }

                foreach (var closestNodeIndex in closestNodes)
                {
                    if (passedNodes.Contains(closestNodeIndex) |
                        toSearchNodes.Contains(closestNodeIndex))
                        continue;

                    toSearchNodes.Add(closestNodeIndex);
                    Vector2Int nodePos = GetVoxel2D(closestNodeIndex, mapSize);

                    pathMap[nodePos.x, nodePos.y] = currentNodeIndex;
                    costsMap[nodePos.x, nodePos.y] = CalculateCost(GetVoxel2D(fromIndex, mapSize), nodePos, GetVoxel2D(toIndex, mapSize));
                }
            }

            int[,] path = new int[mapSize.x, mapSize.y];

            for (int y = 0; y < mapSize.y; y++)
                for (int x = 0; x < mapSize.x; x++)
                {
                    path[x, y] = y * mapSize.y + x;
                }

            int nind = currentNodeIndex;

            while (nind != fromIndex)
            {
                Vector2Int voxpos = GetVoxel2D(nind, mapSize);
                path[voxpos.x, voxpos.y] = pathMap[voxpos.x, voxpos.y];
                nind = pathMap[voxpos.x, voxpos.y];
            }

            conns = path;
            costs = costsMap;
        }

        public class PathfindingRules
        {

        }

        public static float CalculateCost(Vector2Int start, Vector2Int current, Vector2Int target)
        {
            return Vector2.Distance(start, current) * Vector2.Distance(current, target);
        }

        public static Vector2Int GetVoxel2D(in int index, in Vector2Int arraySize)
        {
            return new(index % arraySize.x, index / arraySize.x);
        }

        public static int[] GetClosestNodes(in Vector2Int node, in Vector2Int mapSize)
        {
            List<int> nodes = new();

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Vector2Int voxel = new Vector2Int(x - 1, y - 1) + node;
                    
                    if (IsInsideOfChunk(voxel, mapSize))
                        nodes.Add((y + node.y - 1) * mapSize.y + (x + node.x - 1));
                }
            }

            return nodes.ToArray();
        }

        private static bool IsGreaterOrEqual(Vector2Int lhs, Vector2Int rhs)
        {
            return (lhs.x >= rhs.x) && (lhs.y >= rhs.y);
        }

        private static bool IsLesserOrEqual(Vector2Int lhs, Vector2Int rhs)
        {
            return (lhs.x <= rhs.x) && (lhs.y <= rhs.y);
        }

        private static bool IsInsideOfChunk(Vector2Int voxelPosition, Vector2Int mapSize)
        {
            return IsGreaterOrEqual(voxelPosition, Vector2Int.zero) && IsLesserOrEqual(voxelPosition, mapSize - Vector2Int.one);
        }
    }

    private void Awake()
    {
        BuildNavigation();
    }

    [ContextMenu("Generate Map")]
    private void BuildNavigation()
    {
        m_GeneratedMap = new bool[Resolution.x, Resolution.y];

        for (int y = 0; y < Resolution.y; y++)
        {
            for (int x = 0; x < Resolution.x; x++)
            {
                Vector2 position = GetVoxelPosition(x, y);
                m_GeneratedMap[x, y] = Physics2D.OverlapBox(position, Vector2.one, 0.0f, layerMask);
            }
        }
    }

    private int GetClosestVoxel(Vector2 worldPoint, Vector2Int mapSize)
    {
        int currentNodeIndex = -1;
        float minDistance = math.INFINITY;

        Vector2 localPoint = transform.InverseTransformPoint(worldPoint);

        for (int y = 0; y < mapSize.y; y++)
            for (int x = 0; x < mapSize.x; x++)
            {
                Vector2Int voxelPos = new(x, y);
                float distanceToVoxel = Vector2.Distance(localPoint, voxelPos);

                if (distanceToVoxel >= minDistance)
                    continue;

                minDistance = distanceToVoxel;
                currentNodeIndex = y * mapSize.y + x;
            }

        return currentNodeIndex;
    }

    struct BuildNavigationJob : IJob
    {
        [ReadOnly] 
        [WriteOnly] public NativeArray<bool> Map;

        public void Execute()
        {
            
        }
    }

    public void FindPathTo(Vector2 from, Vector2 to)
    {
        Vector2Int mapSize = new(m_GeneratedMap.GetLength(0), m_GeneratedMap.GetLength(1));
        int fromVoxel = GetClosestVoxel(from, mapSize);
        int toVoxel = GetClosestVoxel(to, mapSize);

        PathFinderRules.PathTo(fromVoxel, toVoxel, m_GeneratedMap, out connects, out m_Costs);
    } 

    private Vector2 GetVoxelPosition(int x, int y) => new(transform.position.x + x + 0.5f, transform.position.y + y + 0.5f);

#if UNITY_EDITOR
    private void OnValidate()
    {
        BuildNavigation();
        FindPathTo(m_DebugStartPoint, m_DebugEndPoint);
    }

    private void OnDrawGizmos()
    {
        Vector2Int mapSize = new(m_GeneratedMap.GetLength(0), m_GeneratedMap.GetLength(1));

        Vector2Int startPoint = Vector2Int.zero;
        Vector2Int endPoint = mapSize;

        for (int y = 0; y < mapSize.y; y++)
            for (int x = 0; x < mapSize.x; x++)
            {
                Vector2 position = GetVoxelPosition(x, y);
                Gizmos.color = m_GeneratedMap[x, y] ? Color.grey : Color.white;

                if (m_ShowCosts)
                {
                    Gizmos.color = Color.Lerp(Color.white, Color.black, m_Costs[x, y] / 16); 
                }

                Gizmos.DrawCube(position, Vector3.one * 0.9f);
            }

        if (m_ShowPathMap)
        {
            for (int y = 0; y < mapSize.y; y++)
                for (int x = 0; x < mapSize.x; x++)
                {
                    Vector2Int b = PathFinderRules.GetVoxel2D(connects[x, y], mapSize);
                    Vector2 pointA = GetVoxelPosition(x, y);
                    Vector2 pointB = GetVoxelPosition(b.x, b.y);
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(pointA, pointB);
                }
        }
    }
#endif
}
