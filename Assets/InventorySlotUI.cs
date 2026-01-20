using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventorySlotUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Action onHighlighted;
    public Action onUnHighlighted;

    public GearInstance gearInstance;
    public TextMeshProUGUI itemNameTMP;
    public TextMeshProUGUI itemQuantityTMP;
    public Button button;
    public Color equipmentColor;
    public Color consumableColor;

    public virtual void OnSelect(BaseEventData eventData)
    {
        onHighlighted.Invoke();
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        onUnHighlighted.Invoke();
    }
}
