using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class IMenuGearItemHighlighted : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public MenuManagerUI menuManagerUI;
    public TextMeshProUGUI inventorySlotQuantityTMP;

    public void OnSelect(BaseEventData eventData)
    {
        InventorySlot combatInventorySlot = GetComponent<InventorySlot>();
        string descriptionText = combatInventorySlot.gear.gearDescription;

        menuManagerUI.UpdateDescriptionField(descriptionText, combatInventorySlot.gear.isEquipment);
        inventorySlotQuantityTMP.color = Color.yellow;
    }

    public void OnDeselect(BaseEventData eventData)

    {
        inventorySlotQuantityTMP.color = Color.white;
    }
}


