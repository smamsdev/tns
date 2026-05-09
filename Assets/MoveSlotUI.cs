using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MoveSlotUI : MonoBehaviour, ISelectHandler
{
    public Action onHighlighted;
    public Action onUnHighlighted;
    public MoveSO moveSO;
    public TextMeshProUGUI slotText;
    public Button button;
    public Sprite moveIcon, flawIcon, freeIcon;
    public Image icon;

    public virtual void OnSelect(BaseEventData eventData)
    {
        onHighlighted?.Invoke();
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        onUnHighlighted?.Invoke();
    }
}
