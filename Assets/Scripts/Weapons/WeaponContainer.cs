using UnityEngine;

public sealed class WeaponContainer : MonoBehaviour, IInputReceiver
{
    [SerializeField] private WeaponDataSO WeaponData;
    public WeaponRuntime Weapon { get; private set; }
    InputHandlerInstance IInputReceiver.InputHandler { get => InputHandler; set => InputHandler = value; }
    private InputHandlerInstance InputHandler { get; set; } = new();

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

    void IInputReceiver.OnInputHandlerChanged(in InputHandlerInstance inputHandler)
    {
        InputHandler.OnAttackInitiated += Weapon.Attack;
        InputHandler.OnAttackReleased += Weapon.Release;
    }
}
