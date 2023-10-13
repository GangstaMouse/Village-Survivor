using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modificator
{
    // public float Value;
}

public class MovementModificator
{
    public virtual float Value => 1.0f;
}
