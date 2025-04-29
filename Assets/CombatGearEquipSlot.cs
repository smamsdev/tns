using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatGearEquipSlot : GearEquipSlot
{
    public override void OnSelect(BaseEventData eventData)
    {
        if (gearEquipped != null)
        {
            CombatEvents.UpdateNarrator(gearEquipped.gearDescription);
        }

        else
        {
            CombatEvents.UpdateNarrator("");
        }
    }

    public override void OnDeselect(BaseEventData eventData)
    { 
        //null
    }
}
