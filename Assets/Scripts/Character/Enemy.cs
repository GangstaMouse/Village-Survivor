using UnityEngine;
using Frameworks.BehaviourTree;
using Tree = Frameworks.BehaviourTree.Tree;

public class Enemy : Character
{
    /* public override Vector2 LookDirection { get
    {
        if (Player.Instance == null && Player.Instance.IsAlive)
            return Vector2.zero;

        float3 relativePlayerPosition = Player.Instance.transform.position - transform.position;
        return math.normalizesafe(new float2(relativePlayerPosition.x, relativePlayerPosition.y)); // test
    }} */

    /* public override Vector2 MovementInput { get
    {
        if (Player.Instance == null && Player.Instance.IsAlive)
            return Vector2.zero;

        float3 movementInput3D = math.normalizesafe(Player.Instance.transform.position - transform.position);
        return new(movementInput3D.x, movementInput3D.y);
    }} */

    public override Vector2 MovementInput => dir;

    public override Vector2 LookDirection => lookDir;

    private Tree m_BehaviourTree;

    private Vector2 dir;
    public Vector2 lookDir;

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

    protected override void OnFixedUpdate()
    {
        m_BehaviourTree.Run();
    }
}
