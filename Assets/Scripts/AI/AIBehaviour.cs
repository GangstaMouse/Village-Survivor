using Tree = Frameworks.BehaviourTree.Tree;
using UnityEngine;
using System;

[DisallowMultipleComponent]
[RequireComponent(typeof(Character))]
public abstract class AIBehaviour : MonoBehaviour
{
    [field: SerializeField] public float ReactionTime { get; private set; } = 1.2f; // not implamented yet

    public static event Action<AIBehaviour> OnCreated;
    protected Character m_Character;
    protected Tree m_BehaviourTree;

    protected AIInputHandler aIInputHandler;

    private void Start()
    {
        aIInputHandler = GetComponent<AIInputHandler>();
        m_Character = GetComponent<Character>();
        InitTree();
        OnCreated?.Invoke(this);
    }

    public void UpdateBehaviourTree(float deltaTime)
    {
        if (m_Character.IsAlive)
            m_BehaviourTree.Run();
    }

    protected abstract void InitTree();
}
