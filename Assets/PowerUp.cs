using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private void Start()
    {
        var stats = Player.Instance.Stats;
        stats.AddStat("Damage");
        /* stats.GetStat("Speed").AddModificator(new Modificator(5.0f));
        stats.GetStat("Damage").AddModificator(new Modificator(9.0f)); */
    }
}
