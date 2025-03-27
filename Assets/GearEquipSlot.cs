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

    public virtual void OnSelect(BaseEventData eventData)
    {
        Debug.Log("implement something here");
    }
}