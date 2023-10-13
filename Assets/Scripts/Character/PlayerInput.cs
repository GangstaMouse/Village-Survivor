using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, IAttackInput
{
    [SerializeField] private InputActionReference m_AttackAction;
    Character character;
    private bool Attack;
    private void Awake()
    {
        character = GetComponent<Character>();
    }
    private void OnEnable()
    {
        m_AttackAction.action.performed += (e) => Attack = true;
    }

    public bool IsAttacking()
    {
        bool result = Attack;
        Attack = false;
        return result;
    }
}
