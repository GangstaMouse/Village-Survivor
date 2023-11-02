using Unity.Mathematics;
using UnityEngine;

public interface IDamager
{
    LayerMask LayerMask { get; }
    float3 Origin { get; }
}
