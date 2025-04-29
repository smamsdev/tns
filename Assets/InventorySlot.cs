using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Gear gear;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemQuantity;
    public MenuManagerUI menuManagerUI;
    public MenuGear menuGear;

    public virtual void OnSelect(BaseEventData eventData)
    {
        menuGear.GearHighlighted(gear);

        itemQuantity.color = Color.yellow;
        menuManagerUI.SetTextAlpha(itemQuantity, gear.isCurrentlyEquipped ? 0.5f : 1f);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        itemQuantity.color = Color.white;
        menuManagerUI.SetTextAlpha(itemQuantity, gear.isCurrentlyEquipped ? 0.5f : 1f);
    }
}
