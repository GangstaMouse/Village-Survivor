using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Effect", menuName = "Weapon Effect")]
public class WeaponEffect : ScriptableObject
{
    public void DoEffect(in IDamageSource damageSource, in IDamageble target)
    {
        if (target.GameObject.TryGetComponent(out Burning effect))
            return;

        var newEffect = target.GameObject.AddComponent<Burning>();
        newEffect.Init(damageSource);
    }
}
