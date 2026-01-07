using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GearSO gear;
    public TextMeshProUGUI itemNameTMP;
    public TextMeshProUGUI itemQuantityTMP;
    public MenuGearInventorySubPage menuGearInventorySubPage;
    public Button button;

    public virtual void OnSelect(BaseEventData eventData)
    {
        menuGearInventorySubPage.GearSlotHighlighted(this);
        itemQuantityTMP.color = Color.yellow;
        FieldEvents.SetTextAlpha(itemNameTMP, gear.isCurrentlyEquipped ? 0.5f : 1f);
        FieldEvents.SetTextAlpha(itemQuantityTMP, gear.isCurrentlyEquipped ? 0.5f : 1f);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        itemQuantityTMP.color = Color.white;
        FieldEvents.SetTextAlpha(itemNameTMP, gear.isCurrentlyEquipped ? 0.5f : 1f);
        FieldEvents.SetTextAlpha(itemQuantityTMP, gear.isCurrentlyEquipped ? 0.5f : 1f);
    }
}
