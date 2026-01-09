using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class IShopGearItemHighlighted : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    //fuckin delete this


    [SerializeField] ShopMenuManagerUI shopManagerUI;
    public TextMeshProUGUI inventorySlotQuantityTMP;

    public void OnSelect(BaseEventData eventData)
    {
        InventorySlotUI combatInventorySlot = GetComponent<InventorySlotUI>();
        shopManagerUI.UpdateDescriptionField(combatInventorySlot.gear);
        inventorySlotQuantityTMP.color = Color.yellow;
    }

    public void OnDeselect(BaseEventData eventData)

    {
        inventorySlotQuantityTMP.color = Color.white;
    }
}


