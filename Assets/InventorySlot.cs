using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public Gear gear;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemQuantity;

    public void DisplayInventoryItemDescription()

    {
        CombatEvents.UpdateNarrator(gear.gearDescription);
    }
}
