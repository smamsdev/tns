using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GearEquipSlot : MonoBehaviour, ISelectHandler
{
    public Gear gearEquipped;
    public TextMeshProUGUI buttonTMP;
    public int equipSlotNumber;
    public MenuGearEquip menuGearEquip;

    public void OnSelect(BaseEventData eventData)
    {
        menuGearEquip.EquipSlotHighlighted(this);
    }
}