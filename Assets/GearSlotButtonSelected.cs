using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GearSlotButtonSelected : MonoBehaviour, ISelectHandler
{
    int gearSlotNumber;

    public void OnSelect(BaseEventData eventData)
    {
        gearSlotNumber = int.Parse(this.gameObject.name);
        CombatEvents.GearSlotButtonHighlighted?.Invoke(gearSlotNumber);
    }
}