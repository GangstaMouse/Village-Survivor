using Frameworks.BehaviourTree;
using Tree = Frameworks.BehaviourTree.Tree;
using UnityEngine;

public class MageGoblinAIBehaviour : AIBehaviour
{
    [field: SerializeField] public float ReactionTime { get; private set; } = 1.2f; // not implamented yet

    protected override void InitTree()
    {
        m_BehaviourTree = new Tree
        (
            new Selector(new()
            {
                new Sequence(new()
                {
                    new MageHoldDistance(m_Character, (AIInputHandlerInst)InputHandler),
                    new IsPlayerAttackRange(m_Character, (AIInputHandlerInst)InputHandler)
                }),
                new Sequence(new()
                {
                    new EnemyFleeDeco(m_Character, (AIInputHandlerInst)InputHandler),
                    new EnemyFleeTask(m_Character, (AIInputHandlerInst)InputHandler)
                })
            })
        );
    }
}
