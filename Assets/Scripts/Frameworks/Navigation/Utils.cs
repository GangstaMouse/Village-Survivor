using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Frameworks.Navigation
{
    public static class BurstUtils
    {
        // Maybe it's can be optimized
        public static int GetClosestVoxel(Vector2 worldPoint, NavigationGrid2D navigationGrid)
        {
            int2 navigationBounds = new(navigationGrid.GridSize.x, navigationGrid.GridSize.y);
            NativeArray<bool> navigationObstacles = new(navigationBounds.x * navigationBounds.y, Allocator.TempJob);
            for (int i = 0; i < navigationObstacles.Length; i++)
                navigationObstacles[i] = Convert.ToBoolean(navigationGrid.ObstaclesData[i]);

            NativeArray<int> index = new(1, Allocator.TempJob);

            var job = new GetClosestVoxelJob()
            {
                WorldPoint = new(worldPoint.x, worldPoint.y, 0.0f),
                NavigationBounds = navigationBounds,
                NavigationObstacles = navigationObstacles,
                Transform = navigationGrid.Transform.localToWorldMatrix,
                Index = index
            };

            job.Schedule().Complete();

            navigationObstacles.Dispose();

            int result = index[0];
            index.Dispose();
            return result;
        }

        [BurstCompile]
        // Maybe it's can be optimized
        private struct GetClosestVoxelJob : IJob
        {
            [ReadOnly] public float3 WorldPoint;
            [ReadOnly] public int2 NavigationBounds;
            [ReadOnly] public NativeArray<bool> NavigationObstacles;
            [ReadOnly] public float4x4 Transform;
            [WriteOnly] public NativeArray<int> Index;

            public void Execute()
            {
                int currentNodeIndex = -1;
                float minDistance = math.INFINITY;

                float3 localPoint = math.transform(math.inverse(Transform), WorldPoint);
                float2 localPoint2D = new(localPoint.x, localPoint.y);
                float2 displacement = new(-NavigationBounds.x / 2.0f, -NavigationBounds.y / 2.0f);

                for (int y = 0; y < NavigationBounds.y; y++)
                    for (int x = 0; x < NavigationBounds.x; x++)
                    {
                        bool isBlocked = NavigationObstacles[y * NavigationBounds.x + x];
                        float2 voxelPos = new(x + displacement.x, y + displacement.y);
                        float distanceToVoxel = math.distance(localPoint2D, voxelPos);

                        if (distanceToVoxel >= minDistance || isBlocked)
                            continue;

                        minDistance = distanceToVoxel;
                        currentNodeIndex = y * NavigationBounds.x + x;
                    }

                Index[0] = currentNodeIndex;
            }
        }
    }

    public static class Utils
    {
        public static float SimpleReachCostCalculator(int start, int current, int target, Vector2Int mapSize)
        {
            var startPoint = GetLocalVoxelFromIndex(start, mapSize);
            var currentPoint = GetLocalVoxelFromIndex(current, mapSize);
            var targetPoint = GetLocalVoxelFromIndex(target, mapSize);
            return SimpleReachCostCalculator(startPoint, currentPoint, targetPoint);
        }

        public static float SimpleReachCostCalculator(Vector2Int start, Vector2Int current, Vector2Int target)
        {
            // return Vector2Int.Distance(start, current) * Vector2Int.Distance(current, target);
            return Vector2Int.Distance(current, target);
        }

        // public static float AdvancedCost()

        public static Vector2Int GetLocalVoxelFromIndex(in int index, in Vector2Int arraySize)
        {
            return new(index % arraySize.x, index / arraySize.x);
        }

        public static int[] GetNeightborNodes(in Vector2Int node, in Vector2Int mapSize)
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

        public static bool IsGreaterOrEqual(Vector2Int lhs, Vector2Int rhs)
        {
            return (lhs.x >= rhs.x) && (lhs.y >= rhs.y);
        }

        public static bool IsLesserOrEqual(Vector2Int lhs, Vector2Int rhs)
        {
            return (lhs.x <= rhs.x) && (lhs.y <= rhs.y);
        }

        public static bool IsInsideOfChunk(Vector2Int voxelPosition, Vector2Int mapSize)
        {
            return IsGreaterOrEqual(voxelPosition, Vector2Int.zero) && IsLesserOrEqual(voxelPosition, mapSize - Vector2Int.one);
        }

        public static int GetClosestVoxel(Vector2 worldPoint, NavigationGrid2D navigationGrid)
        {
            int currentNodeIndex = -1;
            float minDistance = math.INFINITY;

            Vector2 localPoint = navigationGrid.Transform.InverseTransformPoint(worldPoint);
            Vector2 displacement = new(-navigationGrid.GridSize.x / 2.0f, -navigationGrid.GridSize.y / 2.0f);

            for (int y = 0; y < navigationGrid.GridSize.y; y++)
                for (int x = 0; x < navigationGrid.GridSize.x; x++)
                {
                    bool isBlocked = Convert.ToBoolean(navigationGrid.ObstaclesData[y * navigationGrid.GridSize.x + x]);
                    Vector2 voxelPos = new(x + displacement.x, y + displacement.y);
                    float distanceToVoxel = Vector2.Distance(localPoint, voxelPos);

                    if (distanceToVoxel >= minDistance || isBlocked)
                        continue;

                    minDistance = distanceToVoxel;
                    currentNodeIndex = y * navigationGrid.GridSize.x + x;
                }

            return currentNodeIndex;
        }

        public static Vector2 LocalToWorldVoxel(in Vector2Int voxelPos, in NavigationGrid2D navigationGrid) =>
            LocalToWorldVoxel(voxelPos, navigationGrid.GridSize, navigationGrid.Transform);

        public static Vector2 LocalToWorldVoxel(in Vector2Int voxelPos, in Vector2 gridSize, in Transform transform)
        {
            Vector2 displacement = new(-gridSize.x / 2.0f, -gridSize.y / 2.0f);
            return new(transform.position.x + voxelPos.x + 0.5f + displacement.x, transform.position.y + voxelPos.y + 0.5f + displacement.y);
        }
    }
}
