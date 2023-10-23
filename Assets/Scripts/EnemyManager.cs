using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float BehaviourUpdateRate = 0.1f;
    public static EnemyManager Instance { get; private set; }
    private List<Enemy> m_EnemyPool = new();
    private bool m_IsRunning;
    private float timer;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Enemy.OnCreated += (e) => m_EnemyPool.Add(e);
        Start();
    }

    private async void Start()
    {
        m_IsRunning = true;

        while (m_IsRunning)
        {
            await Task.Delay((int)(1000 * BehaviourUpdateRate));

            foreach (var enemy in m_EnemyPool)
            {
                float deltaTime = Time.fixedDeltaTime;

                enemy.UpdateBehaviourTree(deltaTime);
            }

            await Task.Yield();
        }
    }

    private void OnDisable()
    {
        m_IsRunning = false;
    }

    private void FixedUpdate()
    {
        /* timer -= Time.fixedDeltaTime / BehaviourUpdatesPerSecond;

        if (timer >= 0)
            return;

        float time = 1.0f / BehaviourUpdatesPerSecond;
        timer = time;

        foreach (var enemy in EnemyPool)
        {
            float deltaTime = Time.fixedDeltaTime;

            enemy.UpdateBehaviourTree(deltaTime);
        } */
    }
}
