using Frameworks.BehaviourTree;
using Unity.Mathematics;
using UnityEngine;

public class MageHoldDistance : CharacterAIBaseNode
{
    public MageHoldDistance(in Character controller, in AIInputHandler inputHandler) : base(controller, inputHandler)
    {
    }

    public override NodeState Evaluate()
    {
        float distanceToPlayer = math.distance((Vector2)Player.Instance.transform.position, (Vector2)m_Controller.transform.position);

        Vector2 targetDir = ((Vector2)m_Controller.transform.position - (Vector2)Player.Instance.transform.position).normalized;
        Vector2 localTarget = targetDir * (6.0f - distanceToPlayer);
        m_InputHandler.SetLook((Vector2)Player.Instance.transform.position - (Vector2)m_Controller.transform.position);
        m_InputHandler.SetRelativeMovementDestination(localTarget);

        if (distanceToPlayer >= 5.0f)
        {
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
