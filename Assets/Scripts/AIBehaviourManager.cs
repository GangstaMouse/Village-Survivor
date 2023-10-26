using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AIBehaviourManager : MonoBehaviour
{
    [SerializeField] private float BehaviourUpdateRate = 0.1f;
    public static AIBehaviourManager Instance { get; private set; }
    private List<AIBehaviour> m_AIBehavioursPool = new();
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
        AIBehaviour.OnCreated += (e) => m_AIBehavioursPool.Add(e);
        Start();
    }

    private async void Start()
    {
        m_IsRunning = true;

        while (m_IsRunning)
        {
            await Task.Delay((int)(1000 * BehaviourUpdateRate));

            foreach (var behaviour in m_AIBehavioursPool)
            {
                float deltaTime = Time.fixedDeltaTime;

                behaviour.UpdateBehaviourTree(deltaTime);
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
