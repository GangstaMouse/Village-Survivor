using Frameworks.BehaviourTree;

public sealed class EnemyFleeDeco : Node
{
    private Enemy m_Controller;

    public EnemyFleeDeco(Enemy controller) => m_Controller = controller;

    public override NodeState Evaluate()
    {
        if (m_Controller.Health <= 3.2f)
        {
            foreach (var node in m_ChildNodes)
                node.Evaluate();

            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
