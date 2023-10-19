using Unity.Mathematics;
using Frameworks.BehaviourTree;

public sealed class IsPlayerAttackRange : Node
{
    private Enemy m_Controller;

    public IsPlayerAttackRange(Enemy controller) => m_Controller = controller;

    public override NodeState Evaluate()
    {
        if (math.distance(m_Controller.transform.position, Player.Instance.transform.position) <= m_Controller.Weapon.AttackRange)
        {
            m_Controller.Attk();
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
