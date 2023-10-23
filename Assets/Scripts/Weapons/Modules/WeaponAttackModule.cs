using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Attack Module", menuName = "Weapon Modules/Attack Module")]
public class WeaponAttackModule : ScriptableObject
{
    [field: SerializeField] public int Repeats { get; private set; } = 1;
    [field: SerializeField] public float Delay { get; private set; } = 0.0f;
    public async void Initiate(Action attackAction, Action attackOverAction)
    {
        int iterations = 0;

        while (iterations < Repeats)
        {
            await Task.Delay((int)(1000 * Delay));

            attackAction.Invoke();

            iterations++;
            await Task.Yield();
        }
        
        attackOverAction.Invoke();
    }
}
