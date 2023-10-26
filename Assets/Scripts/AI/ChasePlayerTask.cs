using UnityEngine;
using Frameworks.BehaviourTree;

public sealed class ChasePlayerTask : CharacterAIBaseNode
{
    public ChasePlayerTask(in Character controller, in AIInputHandlerInst inputHandler) : base(controller, inputHandler)
    {
    }

    public override NodeState Evaluate()
    {
        if (Player.Instance == null || Player.Instance.Character.IsAlive == false)
        {
            m_InputHandler.SetRelativeMovementDestination(Vector2.zero);
            return NodeState.Failure;
        }

        m_InputHandler.SetLook((Vector2)Player.Instance.transform.position - (Vector2)m_Controller.transform.position);
        m_InputHandler.SetRelativeMovementDestination((Vector2)Player.Instance.transform.position - (Vector2)m_Controller.transform.position);

        foreach (var child in m_ChildNodes)
            child.Evaluate();

        return NodeState.Running;
    }
}
