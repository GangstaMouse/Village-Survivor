using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DecalManager : MonoBehaviour
{
    private static List<DecalRuntime> m_Decals = new();
    private static bool m_Running;

    private void OnEnable() => ProcessDecals();
    private void OnDisable() => m_Running = false;

    // not working
    private static async void ProcessDecals()
    {
        m_Running = true;

        while (m_Running)
        {
            List<DecalRuntime> decalsCopy = new(m_Decals);
            float currentTime = Time.realtimeSinceStartup;

            foreach (var decal in decalsCopy)
            {
                float decalLifeTime = currentTime - decal.TimeSinceCreated;

                Color colorCopy = decal.Renderer.color;
                colorCopy.a = decal.LifeTime - decalLifeTime;
                decal.Renderer.color = colorCopy;

                if (decalLifeTime >= decal.LifeTime)
                {
                    Destroy(decal.Renderer.gameObject);
                    m_Decals.Remove(decal);
                }
            }

            await Task.Yield();
        }
    }

    public static DecalRuntime CreateInstance(DecalDataSO decalData, Vector3 position)
    {
        GameObject decalObject = new("Decal", typeof(SpriteRenderer));
        decalObject.transform.position = position;
        
        SpriteRenderer spriteRenderer = decalObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = decalData.Sprite;
        spriteRenderer.color = decalData.Color;
        spriteRenderer.sortingLayerName = "Decal";

        DecalRuntime decalRuntime = new(spriteRenderer, Time.realtimeSinceStartup);
        m_Decals.Add(decalRuntime);
        return decalRuntime;
    }

    public static DecalRuntime CreateInstance(DecalDataSO decalData, Vector3 position, float lifeTime)
    {
        DecalRuntime decal = CreateInstance(decalData, position);
        decal.LifeTime = lifeTime;
        return decal;
    }

    public class DecalRuntime
    {
        public readonly SpriteRenderer Renderer;
        public readonly float TimeSinceCreated;
        public float LifeTime = 1.0f;

        public DecalRuntime(SpriteRenderer renderer, float timeSineCreate)
        {
            Renderer = renderer;
            TimeSinceCreated = timeSineCreate;
        }
    }
}
