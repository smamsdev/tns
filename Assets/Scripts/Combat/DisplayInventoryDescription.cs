using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInventoryDescription : MonoBehaviour
{
    [SerializeField] CombatInventoryMenu combatInventoryMenu;

    private void OnEnable()
    {
        CombatEvents.InventoryButtonHighlighted += ButtonHighlighted;
    }

    private void OnDisable()
    {
        CombatEvents.InventoryButtonHighlighted -= ButtonHighlighted;
    }

    void ButtonHighlighted(int inventorySlotNumber)

    {
        if (combatInventoryMenu.inventorySlot[inventorySlotNumber].gear != null)
        {
            combatInventoryMenu.inventorySlot[inventorySlotNumber].DisplayInventoryItemDescription();
        }
    }
}
