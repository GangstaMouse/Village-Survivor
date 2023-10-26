using Unity.Mathematics;

class AIInputHandler : InputHandler<AIInputHandlerInst>
{
    protected override void Initialize()
    {
        InputHandlerInstance = new(GetComponent<Character>());
    }
}

public class AIInputHandlerInst : InputHandlerInstance
{
    private readonly Character m_Character;

    public AIInputHandlerInst(Character character)
    {
        m_Character = character;
    }

    internal void InitiateAttack() => RaiseOnAttackInitiated();
    internal void ReleaseAttack() => RaiseOnAttackReleased();

    public void SetLook(float2 relativeLook)
    {
        LookingDirection = math.normalizesafe(relativeLook);
    }

    public void SetRelativeMovementDestination(float2 relativePoint)
    {
        float distance = math.length(relativePoint);
        float2 direction = math.normalizesafe(relativePoint);
        MovementInput = direction * (math.min(distance, m_Character.MovementSpeed) / m_Character.MovementSpeed);
    }
}
