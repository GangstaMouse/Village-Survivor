using UnityEngine;

public abstract class AttackAbility : ScriptableObject
{
    public float Damage = 2;
    public float AttackRange = 1.4f;
    public float AttackRadius = 0.5f;
    public float CoolDown = 0.5f;
    [SerializeField] private AudioCollection attackSound;
    public void Attack(in Character characterContext)
    {
        OnAttack(characterContext);

        if (attackSound)
            AudioManager.CreateAudioInstance(attackSound, characterContext.transform.position);
    }
    protected abstract void OnAttack(in Character characterContext);
}


