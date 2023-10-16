using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct SpawnerParameters
{
    public float Rate;
    public float EnemyLimit;
}

public static class MobSpawner
{
    private static float m_SpawnRange;
    private const float m_SpawnRangeOffset = 5.0f;

    private static CancellationTokenSource m_TokenSource;
    private static CancellationToken m_Token;

    private static List<GameObject> m_SpawnedMobs = new();
    private static SpawnerParameters m_Parameters;

    public static void Init()
    {
        Camera camera = MonoBehaviour.FindObjectOfType<Camera>();
        Vector3 maxPoint = camera.ScreenToWorldPoint(new Vector3(Screen.currentResolution.width, Screen.currentResolution.height, 1));
        m_SpawnRange = math.lengthsq(new Vector2(maxPoint.x, maxPoint.y));

        m_TokenSource = new();
    }

    public static void Enable(SpawnerParameters parameters, List<GameObject> mobs)
    {
        m_Parameters = parameters;
        m_TokenSource.Cancel();

        m_TokenSource = new();
        m_Token = m_TokenSource.Token;
        OnEnabled(mobs, m_Token);
    }

    private static async void OnEnabled(List<GameObject> mobs, CancellationToken newToken)
    {
        Debug.LogWarning("Mob spawner Enabled!");

        while(Player.Instance && Player.Instance.IsAlive)
        {
            if (newToken.IsCancellationRequested)
                return;

            if (m_Parameters.EnemyLimit == 0 || m_SpawnedMobs.Count < m_Parameters.EnemyLimit)
            {
                Vector3 playerPosition = Player.Instance.transform.position;

                int index = UnityEngine.Random.Range(0, mobs.Count - 1);
                float angle = math.degrees(UnityEngine.Random.Range(0.0f, 1.0f));
                Vector3 direction = math.mul(quaternion.AxisAngle(math.forward(), angle), math.up());

                var newMob = SpawnMob(mobs[index], playerPosition + (direction * (m_SpawnRange + m_SpawnRangeOffset)));
                m_SpawnedMobs.Add(newMob);

                await Task.Delay((int)math.round(1000 / m_Parameters.Rate));
            }

            await Task.Yield();
        }
    }

    public static void Disable()
    {
        m_TokenSource.Cancel();
        OnDisbled();
    }

    private static void OnDisbled() { }

    public static GameObject SpawnMob(GameObject gameObject, float3 location) => MonoBehaviour.Instantiate(gameObject, location, quaternion.identity);

    public static void ResetSpawner()
    {
        foreach (var mob in m_SpawnedMobs)
            MonoBehaviour.DestroyImmediate(mob);
    }
}