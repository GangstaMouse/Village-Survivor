using System.Threading.Tasks;

// Вертикальный срез
// [Serializable]
public sealed class DamageEffect
{
    public float Damage = 1.0f;
    public float LifeTime = 3.0f;
    public readonly AudioCollection audioCollection;
    public readonly IDamageble damageble;

    public DamageEffect(float damage, float lifeTime, AudioCollection audioCollection, IDamageble damageble)
    {
        Damage = damage;
        LifeTime = lifeTime;
        this.damageble = damageble;
        Activate();
    }

    private async void Activate()
    {
        while (LifeTime > 0)
        {
            ImpactSystem.Impact(new() { Damage = Damage, effects = new() }, damageble, null);
            AudioManager.CreateAudioInstance(audioCollection, damageble.Position);
            await Task.Delay(1000);
            LifeTime -= 1.0f;

            await Task.Yield();
        }

        ImpactSystem.OnEffectLifeTimeOver(this, damageble);
    }
}
