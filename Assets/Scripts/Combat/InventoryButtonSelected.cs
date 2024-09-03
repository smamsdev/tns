using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryButtonSelected : MonoBehaviour, ISelectHandler
{
    int inventorySlotNumber;

    public void OnSelect(BaseEventData eventData)
    {
        inventorySlotNumber =  int.Parse(this.gameObject.name);
        CombatEvents.InventoryButtonHighlighted?.Invoke(inventorySlotNumber);
    }
}