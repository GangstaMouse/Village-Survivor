using Unity.Mathematics;
using UnityEngine;

public interface IStats
{
    
}

public sealed class Weapon : MonoBehaviour, IDamagerDirect, IInputReceiver
{
    [SerializeField] private WeaponDataSO WeaponData;
    public WeaponRuntime WeaponRuntime { get; private set; }
    InputHandlerInstance IInputReceiver.InputHandler { get => InputHandler; set => InputHandler = value; }
    private InputHandlerInstance InputHandler { get; set; } = new();

    [SerializeField] private LayerMask m_AttackLayerMask;

    public LayerMask LayerMask => m_AttackLayerMask;
    float3 IDamager.Origin => transform.position;
    float3 IDamagerDirect.Direction => transform.up;

    private void Awake()
    {
        WeaponRuntime = WeaponData.CreateRuntime(this);
    }

    private void FixedUpdate()
    {
        WeaponRuntime.Tick(Time.fixedDeltaTime);
    }

    void IInputReceiver.OnInputHandlerChanged(in InputHandlerInstance inputHandler)
    {
        InputHandler.OnAttackInitiated += WeaponRuntime.Attack;
        InputHandler.OnAttackReleased += WeaponRuntime.Release;
    }
}
