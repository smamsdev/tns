using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, ISelectHandler
{
    public Gear gear;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemQuantity;

    public virtual void OnSelect(BaseEventData eventData)

    {
        Debug.Log("remember to add some menu display update logic here pls");
    }
}
