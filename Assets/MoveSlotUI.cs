using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MoveSlotUI : MonoBehaviour, ISelectHandler
{
    //wtf is this for
    //public enum MoveArrayType {NotSelected, ViolentAttacks, ViolentFends, ViolentFocuses, CautiousAttacks, CautiousFends, CautiousFocuses, PreciseAttacks, PrecisesFends, PrecisesFocuses};
    // public MoveArrayType moveArrayType = MoveArrayType.NotSelected;

    public Action onHighlighted;
    public Action onUnHighlighted;
    public MoveSO moveSO;
    public TextMeshProUGUI slotText;
    public Button button;

    public virtual void OnSelect(BaseEventData eventData)
    {
        onHighlighted.Invoke();
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        onUnHighlighted.Invoke();
    }
}
