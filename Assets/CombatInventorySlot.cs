using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatInventorySlot : MonoBehaviour
{
    public Gear gear;
    public TextMeshProUGUI textMeshProUGUI;

    public void DisplayInventoryItemDescription()

    {
        CombatEvents.UpdateNarrator(gear.gearDescription);
    }
}
