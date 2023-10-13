using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackAbility : ScriptableObject
{
    public abstract void OnAttack(in Character characterContext);
}


