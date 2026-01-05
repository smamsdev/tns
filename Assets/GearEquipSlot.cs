using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIGearEquipSlot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GearSO gearEquipped;
    public TextMeshProUGUI buttonTMP;
    public int equipSlotNumber;
    public MenuGearEquipSubPage menuGearEquip;

    public virtual void OnSelect(BaseEventData eventData)
    {
        menuGearEquip.EquipSlotHighlighted(this);
        buttonTMP.color = Color.yellow;
    }

    public virtual void OnDeselect(BaseEventData eventData) 
    {
        buttonTMP.color = Color.white;
    }

    public void Deselect()
    {
        buttonTMP.color = Color.white;
    }
}