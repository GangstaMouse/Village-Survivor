using Unity.Mathematics;
using UnityEngine;
using Frameworks.BehaviourTree;

public sealed class EnemyFleeTask : Node
{
    private Enemy m_Controller;

    public EnemyFleeTask(Enemy controller) => m_Controller = controller;

    public override NodeState Evaluate()
    {
        if (Player.Instance == null)
            return NodeState.Failure;

        float3 movementInput3D = math.normalizesafe(m_Controller.transform.position - Player.Instance.transform.position);
        Vector2 movementInput = new(movementInput3D.x, movementInput3D.y);
        m_Controller.Move(movementInput);

        return NodeState.Running;
    }
}
