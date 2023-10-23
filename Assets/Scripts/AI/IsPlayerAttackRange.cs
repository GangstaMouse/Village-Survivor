using Unity.Mathematics;
using Frameworks.BehaviourTree;

public sealed class IsPlayerAttackRange : Node
{
    private Enemy m_Controller;
    private WeaponContainer weaponContainer;

    public IsPlayerAttackRange(Enemy controller)
    {
        m_Controller = controller;
        weaponContainer = m_Controller.GetComponent<WeaponContainer>();
    }

    public override NodeState Evaluate()
    {
        if (math.distance(m_Controller.transform.position, Player.Instance.transform.position) <= weaponContainer.Weapon.AttackRange)
        {
            weaponContainer.InitiateAttack();
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
