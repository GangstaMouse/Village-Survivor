using Unity.Mathematics;
using UnityEngine;
using Frameworks.BehaviourTree;

public sealed class EnemyFleeTask : CharacterAIBaseNode
{
    public EnemyFleeTask(in Character controller, in AIInputHandlerInst inputHandler) : base(controller, inputHandler)
    {
    }

    public override NodeState Evaluate()
    {
        if (Player.Instance == null)
            return NodeState.Failure;

        m_InputHandler.SetLook((Vector2)m_Controller.transform.position - (Vector2)Player.Instance.transform.position);
        m_InputHandler.SetRelativeMovementDestination((Vector2)m_Controller.transform.position - (Vector2)Player.Instance.transform.position);

        return NodeState.Running;
    }
}
