using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GearEquipSlot : MonoBehaviour, ISelectHandler
{
    public Gear gearEquipped;
    public Button button;
    public TextMeshProUGUI buttonTMP;
    public int equipSlotNumber;

    public void OnSelect(BaseEventData eventData)
    {
       // gearSlotNumber = int.Parse(this.gameObject.name);
       // CombatEvents.GearSlotButtonHighlighted?.Invoke(gearSlotNumber);
    }
}