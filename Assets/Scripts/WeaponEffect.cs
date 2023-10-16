using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Effect", menuName = "Weapon Effect")]
public class WeaponEffect : ScriptableObject
{
    public void DoEffect(in IDamageSource damageSource, in IDamageble target)
    {
        if (target.game.TryGetComponent(out Burning effect))
            return;

        var newEffect = target.game.AddComponent<Burning>();
        newEffect.Init(damageSource);
    }
}
