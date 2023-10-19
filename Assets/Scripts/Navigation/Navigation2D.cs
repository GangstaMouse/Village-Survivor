using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Framework.Navigation;
using static Framework.Navigation.Utils;

namespace Framework.Navigation
{
    public struct NavigationGrid2D
    {
        public readonly byte[] ObstaclesData;
        public readonly Vector2Int GridSize;
        public readonly Transform Transform;

        public NavigationGrid2D(byte[] obstaclesData, Vector2Int gridSize, Transform transform)
        {
            ObstaclesData = obstaclesData;
            GridSize = gridSize;
            Transform = transform;
        }

        public Vector2[] FindPath(Vector2 fromWorldPoint, Vector2 toWorldPoint)
        {
            return null;
        }
    }
}

public class Navigation2D : MonoBehaviour
{
    [SerializeField] private Vector2Int m_Resolution = new(16, 16);
    [SerializeField] private LayerMask m_GeneratorLayerMask;
    public static NavigationGrid2D m_GeneratedMap { get; private set; }

    private void Awake() => BuildNavigation();

    [ContextMenu("Generate Map")]
    public void BuildNavigation()
    {
        byte[] obstaclesData = new byte[m_Resolution.x * m_Resolution.y];

        for (int i = 0; i < m_Resolution.x * m_Resolution.y; i++)
        {
            Vector2Int voxelLocalPoint = GetLocalVoxelFromIndex(i, m_Resolution);
            Vector2 voxelWorldPoint = LocalToWorldVoxel(voxelLocalPoint, transform);
            obstaclesData[i] = Convert.ToByte(Physics2D.OverlapBox(voxelWorldPoint, Vector2.one, 0.0f, m_GeneratorLayerMask));
        }

        m_GeneratedMap = new(obstaclesData, m_Resolution, transform);
    }

    public struct NavigationData
    {
        public readonly int[] ReachedFrom;
        public readonly float[] ReachCost;

        public NavigationData(int[] reachedFrom, float[] reachCost)
        {
            ReachedFrom = reachedFrom;
            ReachCost = reachCost;
        }
    }

    public static Vector2[] FindPathTo(Vector2 fromWorldPoint, Vector2 toWorldPoint)
    {
        int fromVoxelIndex = GetClosestVoxel(fromWorldPoint, m_GeneratedMap.GridSize, m_GeneratedMap.Transform);
        int toVoxelIndex = GetClosestVoxel(toWorldPoint, m_GeneratedMap.GridSize, m_GeneratedMap.Transform);

        FindPathTo(fromVoxelIndex, toVoxelIndex, out Vector2Int[] localPath);

        Vector2[] worldPath = new Vector2[localPath.Length];

        for (int i = 0; i < worldPath.Length; i++)
            worldPath[i] = LocalToWorldVoxel(localPath[i], m_GeneratedMap.Transform);

        return worldPath;
    }

    public static void FindPathTo(in int fromVoxelIndex, in int toVoxelIndex, out Vector2Int[] localPathPoints)
    {
        FindPathTo(fromVoxelIndex, toVoxelIndex, out NavigationData navigationData, out int lastReachedVoxelIndex);

        List<Vector2Int> path = new();
        int currentVoxelIndex = lastReachedVoxelIndex;

        while(currentVoxelIndex != fromVoxelIndex)
        {
            Vector2Int voxelLocalPosition = GetLocalVoxelFromIndex(currentVoxelIndex, m_GeneratedMap.GridSize);
            path.Add(voxelLocalPosition);
            currentVoxelIndex = navigationData.ReachedFrom[currentVoxelIndex];
        }

        localPathPoints = path.ToArray();
    }

    public static void FindPathTo(in int fromVoxelIndex, in int toVoxelIndex, out NavigationData navigationData, out int lastReachedVoxelIndex)
    {
        Vector2Int navigationBounds = m_GeneratedMap.GridSize;
        int[] reachedFrom = new int[navigationBounds.x * navigationBounds.y];
        float[] reachCost = new float[navigationBounds.x * navigationBounds.y];

        List<int> passedNodes = new();

        for (int y = 0; y < navigationBounds.y; y++)
            for (int x = 0; x < navigationBounds.x; x++)
            {
                int voxelIndex = y * navigationBounds.x + x;
                reachedFrom[voxelIndex] = -1;
                reachCost[voxelIndex] = math.INFINITY; // remake?

                bool blocked = Convert.ToBoolean(m_GeneratedMap.ObstaclesData[voxelIndex]);

                if (blocked)
                    passedNodes.Add(voxelIndex);
            }

        int currentVoxelIndex = fromVoxelIndex;
        int targetNode = toVoxelIndex;

        List<int> toSearchVoxels = new() { currentVoxelIndex };
        List<int> tempPath = new();

        int iterations = 0;
        bool reached = false;

        while (toSearchVoxels.Count > 0 && reached == false)
        {
            if (iterations > (navigationBounds.x * navigationBounds.y))
                throw new Exception("Infinite loop in the navigation system");

            iterations++;

            currentVoxelIndex = toSearchVoxels[0];
            tempPath.Add(currentVoxelIndex);

            if (currentVoxelIndex == targetNode)
            {
                reached = true;
                break;
            }

            Vector2Int currentNodePosition = GetLocalVoxelFromIndex(currentVoxelIndex, navigationBounds);

            toSearchVoxels.Remove(currentVoxelIndex);
            passedNodes.Add(currentVoxelIndex);

            int[] closestNodes = GetNeightborNodes(currentNodePosition, navigationBounds);

            int bestNode = -1;
            float bestCost = math.INFINITY;
            bool newPathFounded = false;

            foreach (var closestNode in closestNodes)
            {
                if (passedNodes.Contains(closestNode) |
                    toSearchVoxels.Contains(closestNode))
                    continue;

                float cost = SimpleReachCostCalculator(fromVoxelIndex, closestNode, toVoxelIndex, navigationBounds);
                if (cost <= bestCost)
                {
                    newPathFounded = true;
                    bestNode = closestNode;
                    bestCost = cost;
                }
            }

            /* if (newPathFounded == false)
            {
                Debug.LogWarning(tempPath.Count);
                toSearchVoxels.Add(tempPath[tempPath.Count]);
            } */

            if (bestNode == -1)
                continue;

            toSearchVoxels.Insert(0, bestNode);

            reachedFrom[bestNode] = currentVoxelIndex;
            reachCost[bestNode] = bestCost;

            foreach (var closestNode in closestNodes)
            {
                if (passedNodes.Contains(closestNode) |
                    toSearchVoxels.Contains(closestNode))
                    continue;

                // toSearchVoxels.Add(closestNode);
                toSearchVoxels.Insert(1, closestNode);
                reachedFrom[closestNode] = currentVoxelIndex;
                reachCost[closestNode] = SimpleReachCostCalculator(fromVoxelIndex, closestNode, toVoxelIndex, navigationBounds);;
            }
        }
        navigationData = new(reachedFrom, reachCost);
        lastReachedVoxelIndex = currentVoxelIndex;
    }
}
