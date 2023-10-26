using Frameworks.BehaviourTree;

public sealed class EnemyFleeDeco : CharacterAIBaseNode
{
    public EnemyFleeDeco(in Character controller, in AIInputHandlerInst inputHandler) : base(controller, inputHandler)
    {
    }

    public override NodeState Evaluate()
    {
        if (m_Controller.Health <= 1.4f)
        {
            /* foreach (var node in m_ChildNodes)
                node.Evaluate(); */

            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
