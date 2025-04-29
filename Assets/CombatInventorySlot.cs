using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatInventorySlot : InventorySlot 
{
    public Color buttonSelectedYellow;

    public override void OnSelect(BaseEventData eventData)
    {
        CombatEvents.UpdateNarrator(gear.gearDescription);
        SetTextColor(itemQuantity, gear.isCurrentlyEquipped ? 0.7f : 1f);
    }

    public void SetTextColor(TextMeshProUGUI textMeshProUGUI, float alpha)
    {
        Color color = textMeshProUGUI.color;
        color = buttonSelectedYellow;
        color.a = alpha;
        textMeshProUGUI.color = color;
    }
}
