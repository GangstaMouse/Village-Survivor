using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] int point;

#if UNITY_EDITOR
    private float m_MaxReachCost = 0;
#endif
    public struct NavigationMapData
    {
        public int ReachFrom;
        public float ReachCost;

        public NavigationMapData(int reachFrom, float reachCost)
        {
            ReachFrom = reachFrom;
            ReachCost = reachCost;
        }
    }

    public static class PathFinderRules
    {
        public static void PathTo(in int fromIndex, in int toIndex, in bool[,] map, out int[,] conns, out float[,] costs)
        {
            Vector2Int mapSize = new(map.GetLength(0), map.GetLength(1));
            int[,] pathMap = new int[mapSize.x, mapSize.y];
            float[,] costsMap = new float[mapSize.x, mapSize.y];
            NavigationMapData[] navigationData = new NavigationMapData[mapSize.x * mapSize.y];

            List<int> passedNodes = new();

            for (int y = 0; y < mapSize.y; y++)
                for (int x = 0; x < mapSize.x; x++)
                {
                    costsMap[x, y] = math.INFINITY;

                    bool blocked = map[x, y];
                    if (blocked)
                        passedNodes.Add(y * mapSize.x + x);
                }

            int currentNodeIndex = fromIndex;
            int prevNode = currentNodeIndex;
            int targetNode = toIndex;
            Vector2Int grgr = GetVoxel2D(currentNodeIndex, mapSize);
            costsMap[grgr.x, grgr.y] = 0.0f;

            List<int> toSearchNodes = new() { currentNodeIndex };

            int iterations = 0;
            bool reached = false;

            while (toSearchNodes.Count > 0 && reached == false)
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
                    reached = true;

                foreach (var closestNode in closestNodes)
                {
                    if (passedNodes.Contains(closestNode) |
                        toSearchNodes.Contains(closestNode))
                        continue;

                    toSearchNodes.Add(closestNode);
                    Vector2Int nodePos = GetVoxel2D(closestNode, mapSize);

                    navigationData[closestNode] = new()
                    {
                        ReachFrom = currentNodeIndex,
                        ReachCost = SimpleReachCostCalculator(fromIndex, closestNode, toIndex, mapSize)
                    };

                    pathMap[nodePos.x, nodePos.y] = currentNodeIndex;
                    costsMap[nodePos.x, nodePos.y] = SimpleReachCostCalculator(fromIndex, closestNode, toIndex, mapSize);
                }

                // prevNode = currentNodeIndex;
            }

            int[,] path = new int[mapSize.x, mapSize.y];

            for (int y = 0; y < mapSize.y; y++)
                for (int x = 0; x < mapSize.x; x++)
                {
                    path[x, y] = y * mapSize.x + x;
                }

            int nind = currentNodeIndex;

            while (nind != fromIndex)
            {
                Vector2Int voxpos = GetVoxel2D(nind, mapSize);
                path[voxpos.x, voxpos.y] = pathMap[voxpos.x, voxpos.y];
                nind = pathMap[voxpos.x, voxpos.y];
            }

            // conns = path;
            conns = pathMap;
            costs = costsMap;
        }

        public class PathfindingRules
        {

        }

        public static float SimpleReachCostCalculator(int start, int current, int target, Vector2Int mapSize)
        {
            var startPoint = GetVoxel2D(start, mapSize);
            var currentPoint = GetVoxel2D(current, mapSize);
            var targetPoint = GetVoxel2D(target, mapSize);
            return SimpleReachCostCalculator(startPoint, currentPoint, targetPoint);
        }

        public static float SimpleReachCostCalculator(Vector2Int start, Vector2Int current, Vector2Int target)
        {
            return Vector2Int.Distance(start, current) * Vector2Int.Distance(current, target);
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
                        nodes.Add((y + node.y - 1) * mapSize.x + (x + node.x - 1));
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
            for (int x = 0; x < Resolution.x; x++)
            {
                Vector2 position = GetVoxelPosition(x, y);
                m_GeneratedMap[x, y] = Physics2D.OverlapBox(position, Vector2.one, 0.0f, layerMask);
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
                currentNodeIndex = y * mapSize.x + x;
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

    public void FindPathTo(Vector2 fromWorldPoint, Vector2 toWorldPoint)
    {
        Vector2Int mapSize = new(m_GeneratedMap.GetLength(0), m_GeneratedMap.GetLength(1));
        int fromIndex = GetClosestVoxel(fromWorldPoint, mapSize);
        int toIndex = GetClosestVoxel(toWorldPoint, mapSize);

        PathFinderRules.PathTo(fromIndex, toIndex, m_GeneratedMap, out connects, out m_Costs);
    }

    public struct Voxel
    {
        public readonly int Index;
        private readonly Vector2Int m_ArraySize;
        public Voxel(int index, Vector2Int arraySize)
        {
            Index = index;
            m_ArraySize = arraySize;
        }
        public Vector2Int position => new(Index % m_ArraySize.x, Index / m_ArraySize.x);
    }

    public Voxel FindClosestVoxel(Vector2 worldPoint, Vector2Int arraySize)
    {
        int currentNodeIndex = -1;
        float minDistance = math.INFINITY;

        Vector2 localPoint = transform.InverseTransformPoint(worldPoint);

        for (int y = 0; y < arraySize.y; y++)
            for (int x = 0; x < arraySize.x; x++)
            {
                Vector2Int voxelPos = new(x, y);
                float distanceToVoxel = Vector2.Distance(localPoint, voxelPos);

                if (distanceToVoxel >= minDistance)
                    continue;

                minDistance = distanceToVoxel;
                currentNodeIndex = y * arraySize.x + x;
            }

        return new Voxel(currentNodeIndex, arraySize);
    }

    // WorldTo >> VoxelIndex || VoxelPosition

    private Vector2 GetVoxelPosition(Vector2Int node) => GetVoxelPosition(node.x, node.y);
    private Vector2 GetVoxelPosition(int x, int y) => new(transform.position.x + x + 0.5f, transform.position.y + y + 0.5f);

#if UNITY_EDITOR
    private void OnValidate()
    {
        BuildNavigation();
        FindPathTo(m_DebugStartPoint, m_DebugEndPoint);

        Vector2Int mapSize = new(m_GeneratedMap.GetLength(0), m_GeneratedMap.GetLength(1));

        // Calculate max reach cost for debug drawings
        for (int y = 0; y < mapSize.y; y++)
            for (int x = 0; x < mapSize.x; x++)
            {
                float cost = m_Costs[x, y];

                if (cost < math.INFINITY && cost > m_MaxReachCost)
                    m_MaxReachCost = cost;
            }
    }

    private void OnDrawGizmos()
    {
        Vector2Int mapSize = new(m_GeneratedMap.GetLength(0), m_GeneratedMap.GetLength(1));
        
        for (int y = 0; y < mapSize.y; y++)
            for (int x = 0; x < mapSize.x; x++)
            {
                Vector2 position = GetVoxelPosition(x, y);
                Gizmos.color = m_GeneratedMap[x, y] ? Color.grey : Color.white;

                if (m_ShowCosts)
                    Gizmos.color = Color.Lerp(Color.white, Color.black, m_Costs[x, y] / m_MaxReachCost);

                Gizmos.DrawCube(position, Vector3.one * 0.9f);
            }

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(GetVoxelPosition(PathFinderRules.GetVoxel2D(GetClosestVoxel(m_DebugStartPoint, mapSize), mapSize)), 0.5f);
        // Gizmos.DrawSphere(GetVoxelPosition(PathFinderRules.GetVoxel2D(point, mapSize)), 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetVoxelPosition(PathFinderRules.GetVoxel2D(GetClosestVoxel(m_DebugEndPoint, mapSize), mapSize)), 0.5f);

        Gizmos.color = Color.yellow;

        if (m_ShowPathMap)
            for (int i = 0; i < connects.Length; i++)
            {
                Vector2Int currentNode = PathFinderRules.GetVoxel2D(i, mapSize);
                Vector2Int prevNode = PathFinderRules.GetVoxel2D(connects[currentNode.x, currentNode.y], mapSize);

                Gizmos.DrawLine(GetVoxelPosition(currentNode), GetVoxelPosition(prevNode));
            }
    }
#endif
}
