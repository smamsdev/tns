using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatInventorySlot : InventorySlot
{
    private void OnEnable()
    {
        Debug.Log(this.gameObject);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        CombatEvents.UpdateNarrator(gear.gearDescription);
    }
}
