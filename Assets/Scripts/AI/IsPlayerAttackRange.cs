using Unity.Mathematics;
using Frameworks.BehaviourTree;

public sealed class IsPlayerAttackRange : CharacterAIBaseNode
{
    private readonly Weapon m_WeaponContainer;

    public IsPlayerAttackRange(in Character controller, in AIInputHandler inputHandler) : base(controller, inputHandler)
    {
        m_WeaponContainer = m_Controller.GetComponent<Weapon>();
    }

    public override NodeState Evaluate()
    {
        /* if (m_InputHandler.IsAttaking)
        {
            m_InputHandler.ReleaseAttack();
            return NodeState.Running;
        } */

        if (math.distance(m_Controller.transform.position, Player.Instance.transform.position) <= m_WeaponContainer.WeaponRuntime.AttackRange)
        {
            m_InputHandler.InitiateAttack();
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
