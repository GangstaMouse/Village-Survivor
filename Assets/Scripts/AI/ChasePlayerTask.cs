using Unity.Mathematics;
using UnityEngine;
using Frameworks.BehaviourTree;

public sealed class ChasePlayerTask : Node
{
    private Enemy m_Controller;

    public ChasePlayerTask(Enemy controller) => m_Controller = controller;

    public override NodeState Evaluate()
    {
        if (Player.Instance == null || Player.Instance.IsAlive == false)
        {
            m_Controller.Move(Vector2.zero);
            return NodeState.Failure;
        }

        m_Controller.lookDir = Looking();
        m_Controller.Move(Movement());

        foreach (var child in m_ChildNodes)
            child.Evaluate();

        return NodeState.Running;
    }

    private Vector2 Looking()
    {
        float3 relativePlayerPosition = Player.Instance.transform.position - m_Controller.transform.position;
        return math.normalizesafe(new float2(relativePlayerPosition.x, relativePlayerPosition.y));
    }

    private Vector2 Movement()
    {
        /* Vector2 pos = Frameworks.Navigation.Navigation2D.GetFlowFieldPath(m_Controller.transform.position)[0];
        float3 movementInput3D = math.normalizesafe(new Vector3(pos.x, pos.y, 0.0f) - m_Controller.transform.position); */
        float3 movementInput3D = math.normalizesafe(Player.Instance.transform.position - m_Controller.transform.position);
        return new(movementInput3D.x, movementInput3D.y);
    }
}
