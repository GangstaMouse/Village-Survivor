using UnityEngine;

public interface ILookInput
{
    public abstract Vector2 GetInput();
}
public interface IMovementInput
{
    public abstract Vector2 GetInput();
}

public interface IAttackInput
{
    public abstract bool IsAttacking();
}
