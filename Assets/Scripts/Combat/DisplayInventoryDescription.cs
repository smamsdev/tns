using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInventoryDescription : MonoBehaviour
{
    [SerializeField] CombatInventoryMenu combatInventoryMenu;

    private void OnEnable()
    {
        CombatEvents.ButtonHighlighted += ButtonHighlighted;
    }

    private void OnDisable()
    {
        CombatEvents.ButtonHighlighted -= ButtonHighlighted;
    }

    void ButtonHighlighted(GameObject gameObject)

    {
        int inventorySlot = int.Parse(gameObject.name);

        if (combatInventoryMenu.inventorySlot[inventorySlot].gear != null)
        {
            combatInventoryMenu.inventorySlot[inventorySlot].DisplayInventoryItemDescription();
        }
    }
}
