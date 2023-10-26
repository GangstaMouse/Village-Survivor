using Frameworks.BehaviourTree;
using Tree = Frameworks.BehaviourTree.Tree;
using UnityEngine;
using System;

[DisallowMultipleComponent]
[RequireComponent(typeof(Character))]
public abstract class AIBehaviour : MonoBehaviour, IInputReceiver
{
    public static event Action<AIBehaviour> OnCreated;
    protected Character m_Character;
    protected Tree m_BehaviourTree;

    InputHandlerInstance IInputReceiver.InputHandler { get => InputHandler; set => InputHandler = value; }
    protected InputHandlerInstance InputHandler;

    private void Start()
    {
        m_Character = GetComponent<Character>();
        InitTree();
        OnCreated?.Invoke(this);
    }

    public void UpdateBehaviourTree(float deltaTime)
    {
        if (m_Character.IsAlive)
            m_BehaviourTree.Run();
    }

    void IInputReceiver.OnInputHandlerChanged(in InputHandlerInstance inputHandler)
    {
        InputHandler = inputHandler;
        InitTree();
    }

    protected abstract void InitTree();
}
