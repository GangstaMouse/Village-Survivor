using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public float SpawnRate = 0.98f;
    [SerializeField] private GameObject mobprefab;

    private void Awake()
    {
        Spawn();
    }

    private async void Spawn()
    {
        while(true)
        {
            await Task.Delay((int)Math.Round(SpawnRate * 1000.0f));

            float angle = math.degrees(UnityEngine.Random.Range(0.0f, 1.0f));

            Vector3 direction = math.mul(quaternion.AxisAngle(math.forward(), angle), math.up());
            float distance = 5.0f;

            if (Player.Instance != null)
            {
                SpawnMob(mobprefab, Player.Instance.transform.position + (direction * distance));
            }

            await Task.Yield();
        }
    }

    public GameObject SpawnMob(GameObject gameObject, float3 location)
    {
        return Instantiate(gameObject, location, quaternion.identity);
    }

    public void ResetSpawner()
    {

    }
}