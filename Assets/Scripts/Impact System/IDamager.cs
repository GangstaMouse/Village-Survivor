using System;

public interface IDamager
{
    public abstract event Action<IDamageble> OnHitCallback;

    public abstract void Init(IDamageSource damageSource);

    /* public void MakeDamage(IDamageSource source, IDamageble damageble)
    {
        
    } */
}