using System;
using Unity.Mathematics;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using static Frameworks.Navigation.Utils;

namespace Frameworks.Navigation
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

    public class Navigation2D : MonoBehaviour
    {
        [SerializeField] private Vector2Int m_Resolution = new(16, 16);
        [SerializeField] private LayerMask m_GeneratorLayerMask;
        public static NavigationGrid2D m_GeneratedMap { get; private set; }
        public static NavigationData flowNav;
        public static Transform target;
        private static Vector3 targetPoint;
        int curPoint;
        int prevPoint;
        private bool m_FlowFieldUpdaterIsRunning;

        private void Awake()
        {
            BuildNavigation();

            CalculateFlowField(GetClosestVoxel(m_GeneratedMap.Transform.position, m_GeneratedMap), m_GeneratedMap, out flowNav);
        }

        /* private void Start()
        {
            if (target != null)
                UpdateFlowField();
        } */

        private async void UpdateFlowField()
        {
            m_FlowFieldUpdaterIsRunning = true;

            while (m_FlowFieldUpdaterIsRunning)
            {
                await Task.Delay(500);

                if (target == null)
                return;

                curPoint = BurstUtils.GetClosestVoxel(target.position, m_GeneratedMap);

                if (curPoint != prevPoint)
                {
                    CalculateFlowField(curPoint, m_GeneratedMap, out flowNav);
                    prevPoint = curPoint;
                }

                await Task.Yield();
            }
        }

        private void OnDisable()
        {
            m_FlowFieldUpdaterIsRunning = false;
        }

        public static Vector2[] GetFlowFieldPath(Vector2 fromWorldPoint)
        {
            int index = BurstUtils.GetClosestVoxel(fromWorldPoint, m_GeneratedMap);

            if (flowNav.ReachedFrom[index] == -1)
                return new[] { fromWorldPoint };

            return new[] { LocalToWorldVoxel(GetLocalVoxelFromIndex(flowNav.ReachedFrom[index], m_GeneratedMap.GridSize), m_GeneratedMap) };
        }

        [ContextMenu("Generate Map")]
        public void BuildNavigation()
        {
            flowNav = new();
            m_GeneratedMap = BuildNavigation(m_Resolution, m_GeneratorLayerMask, transform);
        }

        private static NavigationGrid2D BuildNavigation(in Vector2Int resolution, in LayerMask layerMask, in Transform transform)
        {
            byte[] obstaclesData = new byte[resolution.x * resolution.y];

            for (int i = 0; i < resolution.x * resolution.y; i++)
            {
                Vector2Int voxelLocalPoint = GetLocalVoxelFromIndex(i, resolution);
                Vector2 voxelWorldPoint = LocalToWorldVoxel(voxelLocalPoint, resolution, transform);
                obstaclesData[i] = Convert.ToByte(Physics2D.OverlapBox(voxelWorldPoint, Vector2.one, 0.0f, layerMask));
            }

            return new(obstaclesData, resolution, transform);
        }

        public static void CalculateFlowField(in int targetVoxelIndex, in NavigationGrid2D navigationGrid, out NavigationData navigationData)
        {
            int2 navigationBounds = new(navigationGrid.GridSize.x, navigationGrid.GridSize.y);
            NativeArray<bool> obstaclesData = new(navigationBounds.x * navigationBounds.y, Allocator.TempJob);
            for (int i = 0; i < obstaclesData.Length; i++)
                obstaclesData[i] = Convert.ToBoolean(navigationGrid.ObstaclesData[i]);

            NativeArray<int> reachedFrom = new(navigationBounds.x * navigationBounds.y, Allocator.TempJob);
            NativeArray<float> reachCost = new(navigationBounds.x * navigationBounds.y, Allocator.TempJob);

            NativeList<int> passedVoxels = new(Allocator.TempJob);
            NativeList<int> toSearchVoxels = new(Allocator.TempJob);
            NativeList<int> closestVoxels = new(Allocator.TempJob);

            var job = new FlowFieldJob()
            {
                TargetVoxelIndex = targetVoxelIndex,
                NavigationBounds = navigationBounds,
                ObstaclesData = obstaclesData,
                ReachedFrom = reachedFrom,
                ReachCost = reachCost,
                PassedVoxels = passedVoxels,
                ToSearchVoxels = toSearchVoxels,
                ClosestVoxels = closestVoxels
            };

            job.Schedule().Complete();

            navigationData = new(reachedFrom.ToArray(), reachCost.ToArray());

            obstaclesData.Dispose();

            reachedFrom.Dispose();
            reachCost.Dispose();

            passedVoxels.Dispose();
            toSearchVoxels.Dispose();
            closestVoxels.Dispose();
        }

        [BurstCompile]
        private struct FlowFieldJob : IJob
        {
            [ReadOnly] public int TargetVoxelIndex;
            [ReadOnly] public int2 NavigationBounds;
            [ReadOnly] public NativeArray<bool> ObstaclesData;
            [WriteOnly] public NativeArray<int> ReachedFrom;
            [WriteOnly] public NativeArray<float> ReachCost;
            public NativeList<int> ToSearchVoxels;
            public NativeList<int> PassedVoxels;
            public NativeList<int> ClosestVoxels;

            public void Execute()
            {
                int2 targetVoxelPosition = GetVoxelFromIndex(TargetVoxelIndex);

                for (int y = 0; y < NavigationBounds.y; y++)
                    for (int x = 0; x < NavigationBounds.x; x++)
                    {
                        int voxelIndex = y * NavigationBounds.x + x;
                        ReachedFrom[voxelIndex] = -1;

                        if (ObstaclesData[voxelIndex] == true)
                        {
                            PassedVoxels.Add(voxelIndex);
                            ReachCost[voxelIndex] = math.INFINITY;
                            continue;
                        }

                        int2 currentVoxelPosition = new(x, y);

                        ReachCost[voxelIndex] = CalculateCost(targetVoxelPosition, currentVoxelPosition);
                    }

                ReachCost[TargetVoxelIndex] = 0.0f;
                int currentIndex = TargetVoxelIndex;
                ToSearchVoxels.Add(currentIndex);

                int iterations = 0;
                int iterationsLimit = NavigationBounds.x * NavigationBounds.y;

                while (ToSearchVoxels.Length > 0)
                {
                    if (iterations > iterationsLimit)
                        throw new Exception("Infinite loop in the navigation system job");

                    iterations++;

                    currentIndex = ToSearchVoxels[0];

                    ToSearchVoxels.RemoveAt(0);
                    PassedVoxels.Add(currentIndex);

                    int2 position = GetVoxelFromIndex(currentIndex);
                    GetNeightborNodes(position);

                    foreach (var closestNode in ClosestVoxels)
                    {
                        if (PassedVoxels.Contains(closestNode) |
                            ToSearchVoxels.Contains(closestNode))
                            continue;

                        ToSearchVoxels.Add(closestNode);

                        ReachedFrom[closestNode] = currentIndex;
                    }
                }
            }

            private int2 GetVoxelFromIndex(int index) => new(index % NavigationBounds.x, index / NavigationBounds.x);
            private float CalculateCost(int2 from, int2 to) => math.distance(from, to);

            private void GetNeightborNodes(int2 node)
            {
                ClosestVoxels.Clear();

                for (int y = 0; y < 3; y++)
                    for (int x = 0; x < 3; x++)
                    {
                        int2 voxel = new int2(x - 1, y - 1) + node;

                        if (IsInsideOfChunk(voxel))
                            ClosestVoxels.Add((y + node.y - 1) * NavigationBounds.x + (x + node.x - 1));
                    }
            }

            private static bool IsGreaterOrEqual(int2 lhs, int2 rhs)
            {
                return (lhs.x >= rhs.x) && (lhs.y >= rhs.y);
            }

            private static bool IsLesserOrEqual(int2 lhs, int2 rhs)
            {
                return (lhs.x <= rhs.x) && (lhs.y <= rhs.y);
            }

            private bool IsInsideOfChunk(int2 voxelPosition)
            {
                return IsGreaterOrEqual(voxelPosition, int2.zero) && IsLesserOrEqual(voxelPosition, NavigationBounds - new int2(1, 1));
            }
        }
    }
}
