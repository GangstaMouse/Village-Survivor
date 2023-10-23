using System;
using UnityEngine;

[Flags] public enum CharacterType
{
    Player = 1 << 0,
    NPC = 1 << 1
}

class Pickup : MonoBehaviour
{
    [SerializeField] private StatModificator m_Item;
    [SerializeField] private CharacterType allowedCharacter;

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.TryGetComponent(out Inventoty inventoty))
        {
            inventoty.AddItem(m_Item);
            Destroy(gameObject);
        }
    }
}
