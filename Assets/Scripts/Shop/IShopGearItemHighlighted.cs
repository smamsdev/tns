using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class IShopGearItemHighlighted : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] ShopManagerUI shopManagerUI;
    public TextMeshProUGUI inventorySlotQuantityTMP;

    public void OnSelect(BaseEventData eventData)
    {
        InventorySlot combatInventorySlot = GetComponent<InventorySlot>();
        string descriptionText = combatInventorySlot.gear.gearDescription;

        shopManagerUI.UpdateDescriptionField(descriptionText, combatInventorySlot.gear.isEquipment);
        inventorySlotQuantityTMP.color = Color.yellow;
    }

    public void OnDeselect(BaseEventData eventData)

    {
        inventorySlotQuantityTMP.color = Color.white;
    }
}


