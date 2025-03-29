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

    public virtual void OnSelect(BaseEventData eventData)
    {
        menuManagerUI.UpdateGearDescriptionField(gear);

        itemQuantity.color = Color.yellow;
        menuManagerUI.SetTextAlpha(itemQuantity, gear.isCurrentlyEquipped ? 0.5f : 1f);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        itemQuantity.color = Color.white;
        menuManagerUI.SetTextAlpha(itemQuantity, gear.isCurrentlyEquipped ? 0.5f : 1f);
    }
}
