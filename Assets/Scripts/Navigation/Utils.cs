using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Framework.Navigation
{
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

        public static int GetClosestVoxel(Vector2 worldPoint, Vector2Int mapSize, Transform transform)
        {
            int currentNodeIndex = -1;
            float minDistance = math.INFINITY;

            Vector2 localPoint = transform.InverseTransformPoint(worldPoint);

            for (int y = 0; y < mapSize.y; y++)
                for (int x = 0; x < mapSize.x; x++)
                {
                    bool isBlocked = Convert.ToBoolean(Navigation2D.m_GeneratedMap.ObstaclesData[y * mapSize.x + x]);
                    Vector2Int voxelPos = new(x, y);
                    float distanceToVoxel = Vector2.Distance(localPoint, voxelPos);

                    if (distanceToVoxel >= minDistance || isBlocked)
                        continue;

                    minDistance = distanceToVoxel;
                    currentNodeIndex = y * mapSize.x + x;
                }

            return currentNodeIndex;
        }

        public static Vector2 LocalToWorldVoxel(Vector2Int node, Transform transform) =>
            LocalToWorldVoxel(node.x, node.y, transform);
        public static Vector2 LocalToWorldVoxel(int x, int y, Transform transform) =>
            new(transform.position.x + x + 0.5f, transform.position.y + y + 0.5f);
    }
}
