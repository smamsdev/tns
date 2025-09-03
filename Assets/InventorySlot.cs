using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GearSO gear;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemQuantity;
    public MenuManagerUI menuManagerUI;
    public MenuGear menuGear;
    public Button button;

    public virtual void OnSelect(BaseEventData eventData)
    {
        menuGear.GearSlotHighlighted(this);
        itemQuantity.color = Color.yellow;
        menuManagerUI.SetTextAlpha(itemQuantity, gear.isCurrentlyEquipped ? 0.5f : 1f);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        itemQuantity.color = Color.white;
        menuManagerUI.SetTextAlpha(itemQuantity, gear.isCurrentlyEquipped ? 0.5f : 1f);
    }
}
