using Frameworks.BehaviourTree;
using Tree = Frameworks.BehaviourTree.Tree;

public class MageGoblinAIBehaviour : AIBehaviour
{
    protected override void InitTree()
    {
        m_BehaviourTree = new Tree
        (
            new Selector(new()
            {
                new Sequence(new()
                {
                    new MageHoldDistance(m_Character, aIInputHandler),
                    new IsPlayerAttackRange(m_Character, aIInputHandler)
                }),
                new Sequence(new()
                {
                    new EnemyFleeDeco(m_Character, aIInputHandler),
                    new EnemyFleeTask(m_Character, aIInputHandler)
                })
            })
        );
    }
}
