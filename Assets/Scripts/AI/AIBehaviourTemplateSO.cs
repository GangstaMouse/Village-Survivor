using Frameworks.BehaviourTree;
using Tree = Frameworks.BehaviourTree.Tree;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AIBehaviourTemplateSO : ScriptableObject
{
    [field: SerializeField] public NodeData Root { get; private set; }

    /* public Tree BuildTree()
    {
        Node rootNode = BuildNodes(Root);
        return new(rootNode);
    }

    public Node BuildNodes(NodeData nodeData)
    {
        Node newNode = null;

        foreach (var node in nodeData.NodeDatas)
        {
            switch (node.NodeType)
            {
                case NodeType.ChasePlayerTask:
                        newNode = new ChasePlayerTask(null, null);
                    break;
                default:
                    break;
            }

            newNode.AddChildNode(BuildNodes(node));
        }
        
        return newNode;
    } */
}

[System.Serializable]
public struct NodeData
{
    public NodeType NodeType;
    public List<NodeData> NodeDatas;
}

public enum NodeType
{
    Selector,
    Seq,
    ChasePlayerTask
}
