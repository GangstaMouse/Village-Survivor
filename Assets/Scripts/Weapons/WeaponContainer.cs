using UnityEngine;

public sealed class WeaponContainer : MonoBehaviour
{
    [SerializeField] private WeaponDataSO WeaponData;
    public WeaponRuntime Weapon { get; private set; }

    public void Init()
    {
        Weapon = WeaponData.CreateRuntime(GetComponent<Character>());
    }

    private void Awake()
    {
        Init();
    }

    private void FixedUpdate()
    {
        Weapon.Tick(Time.fixedDeltaTime);
    }

    public void InitiateAttack() => Weapon.Attack();
    public void ReleaseAttack() => Weapon.Release();
}
