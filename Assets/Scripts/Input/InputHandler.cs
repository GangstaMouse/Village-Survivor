using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class InputHandler : MonoBehaviour
{
    private readonly InputHandlerInstance m_InputHandlerInstance = new();

    protected float2 MovementInput { set => m_InputHandlerInstance.MovementInput = value; }
    protected float2 LookingInput { set => m_InputHandlerInstance.LookingDirection = value; }

    protected Action OnAttackInitiated => m_InputHandlerInstance.m_OnAttackInitiated;
    protected Action OnAttackReleased => m_InputHandlerInstance.m_OnAttackReleased;
    protected Action OnDash => m_InputHandlerInstance.m_OnDash;

    protected abstract void Initialize();

    protected virtual void Awake() => Initialize();

    protected virtual void OnEnable()
    {
        List<IInputReceiver> inputReceivers = new(GetComponentsInChildren<IInputReceiver>());

        foreach (var inputReceiver in inputReceivers)
            inputReceiver.SetInputHandler(m_InputHandlerInstance);
    }

    protected virtual void OnDisable()
    {
        List<IInputReceiver> inputReceivers = new(GetComponentsInChildren<IInputReceiver>());

        foreach (var inputReceiver in inputReceivers)
            inputReceiver.SetInputHandler(new InputHandlerInstance());
    }
}

public sealed class InputHandlerInstance
{
    public float2 MovementInput { get; internal set; } = float2.zero;
    public float2 LookingDirection { get; internal set; } = float2.zero;

    // Attack Action
    public bool IsAttaking { get; private set; }
    internal Action m_OnAttackInitiated;
    internal Action m_OnAttackReleased;
    internal Action m_OnDash;

    public event Action OnAttackInitiated
    {
        add { m_OnAttackInitiated += value; }
        remove{ m_OnAttackInitiated -= value; }
    }
    public event Action OnAttackReleased
    {
        add { m_OnAttackReleased += value; }
        remove{ m_OnAttackReleased -= value; }
    }
    public event Action OnDash
    {
        add { m_OnDash += value; }
        remove{ m_OnDash -= value; }
    }
}
