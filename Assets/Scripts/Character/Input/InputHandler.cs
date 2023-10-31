using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
abstract class InputHandler<T> : MonoBehaviour where T : InputHandlerInstance
{
    public T InputHandlerInstance { get; protected set; }

    protected abstract void Initialize();

    private void Awake() => Initialize();
    private void Start()
    {
        List<IInputReceiver> inputReceivers = new(GetComponentsInChildren<IInputReceiver>());

        foreach (var inputReceiver in inputReceivers)
            inputReceiver.SetInputHandler(InputHandlerInstance);
    }
}

public class InputHandlerInstance
{
    public virtual float2 MovementInput { get; protected set; }
    public virtual float2 LookingDirection { get; protected set; }

    // Attack Action
    public bool IsAttaking { get; private set; }
    public event Action OnAttackInitiated;
    public event Action OnAttackReleased;
    public event Action OnDash;

    protected void RaiseOnAttackInitiated()
    {
        IsAttaking = true;
        OnAttackInitiated?.Invoke();
    }

    protected void RaiseOnAttackReleased()
    {
        IsAttaking = false;
        OnAttackReleased?.Invoke();
    }

    protected void Dash() => OnDash?.Invoke();
}
