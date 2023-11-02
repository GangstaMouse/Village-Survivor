using Unity.Mathematics;

public interface IDamagerDirect : IDamager
{
    float3 Direction { get; }
}
