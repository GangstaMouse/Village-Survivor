using UnityEngine;

public interface IDamageSource
{
    public abstract Vector2 Origin { get; }
    public abstract Vector2 Direction { get; }
    public abstract LayerMask LayerMask { get; }
}
