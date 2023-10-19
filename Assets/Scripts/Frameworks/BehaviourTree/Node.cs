using System.Collections.Generic;

namespace Frameworks.BehaviourTree
{
    public enum NodeState
    {
        Success,
        Running,
        Failure
    }

    public abstract class Node
    {
        protected NodeState m_State;
        protected List<Node> m_ChildNodes = new();

        protected Tree m_Tree;

        // public int Priority { get; } = 0;

        public Node() { }
        public Node(List<Node> childNodes) => m_ChildNodes = childNodes;

        public void Init(Tree tree)
        {
            m_Tree = tree;

            foreach (var node in m_ChildNodes)
                node.Init(m_Tree);
        }

        public Node AddChildNode(Node node)
        {
            m_ChildNodes.Add(node);
            return this;
        }

        public abstract NodeState Evaluate();
    }
}
