using System.Collections.Generic;

namespace Frameworks.BehaviourTree
{
    public sealed class Selector : Node
    {
        public Selector(List<Node> childNodes) : base(childNodes) { }

        public override NodeState Evaluate()
        {
            foreach (var node in m_ChildNodes)
                switch (node.Evaluate())
                {
                    case NodeState.Success:
                        return NodeState.Success;

                    case NodeState.Running:
                        continue;

                    case NodeState.Failure:
                        continue;
                    
                    default:
                        continue;
                }

            return NodeState.Failure;
        }
    }
}
