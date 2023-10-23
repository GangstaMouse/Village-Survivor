using UnityEngine;
using Frameworks.BehaviourTree;
using Tree = Frameworks.BehaviourTree.Tree;
using System;

public class Enemy : Character
{
    public override Vector2 MovementInput => dir;

    public override Vector2 LookDirection => lookDir;

    private Tree m_BehaviourTree;

    private Vector2 dir;
    public Vector2 lookDir;
    public static event Action<Enemy> OnCreated;

    public void Attk() => Attack();

    public void Move(Vector2 direction)
    {
        dir = direction;
    }

    private void Awake()
    {
        m_BehaviourTree = new Tree
        (
            new Selector(new()
            {
                new Sequence(new()
                {
                    new ChasePlayerTask(this),
                    new IsPlayerAttackRange(this)
                }),
                new Sequence(new()
                {
                    new EnemyFleeDeco(this),
                    new EnemyFleeTask(this)
                })
            })
        );
    }

    public void UpdateBehaviourTree(float deltaTime)
    {
        if (IsAlive)
            m_BehaviourTree.Run();
    }

    private void OnEnable()
    {
        OnCreated?.Invoke(this);
    }

    protected override void OnFixedUpdate()
    {
    }
}
