using Unity.Mathematics;

public  class AIInputHandler : InputHandler
{
    private Character m_Character;


    protected override void Initialize()
    {
        m_Character = GetComponent<Character>();
    }

    internal void InitiateAttack() => OnAttackInitiated?.Invoke();
    internal void ReleaseAttack() => OnAttackReleased?.Invoke();

    public void SetLook(float2 relativeLook)
    {
        LookingInput = math.normalizesafe(relativeLook);
    }

    public void SetRelativeMovementDestination(float2 relativePoint)
    {
        float distance = math.length(relativePoint);
        float2 direction = math.normalizesafe(relativePoint);
        MovementInput = direction * (math.min(distance, m_Character.MovementSpeed) / m_Character.MovementSpeed);
    }
}
