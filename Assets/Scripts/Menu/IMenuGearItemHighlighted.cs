using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IMenuGearItemHighlighted : MonoBehaviour, ISelectHandler
{
    public MenuGear menuGear;

    public void OnSelect(BaseEventData eventData)
    {
        CombatInventorySlot combatInventorySlot = GetComponent<CombatInventorySlot>();
        string descriptionText = combatInventorySlot.gear.gearDescription;

        menuGear.UpdateDescriptionField(descriptionText, combatInventorySlot.gear.isEquipment);
    }
}


