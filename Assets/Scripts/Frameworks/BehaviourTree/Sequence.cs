using System.Collections.Generic;

namespace Frameworks.BehaviourTree
{
    public sealed class Sequence : Node
    {
        public Sequence(List<Node> childNodes) : base(childNodes) { }

        public override NodeState Evaluate()
        {
            NodeState state = NodeState.Running;

            foreach (var childNode in m_ChildNodes)
                switch (childNode.Evaluate())
                {
                    case NodeState.Running:
                        continue;

                    case NodeState.Success:
                        continue;

                    case NodeState.Failure:
                        return NodeState.Failure;

                    default:
                        return NodeState.Failure;
                }

            return state;
        }
    }
}
