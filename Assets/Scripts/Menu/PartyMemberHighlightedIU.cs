using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PartyMemberHighlightedIU : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Action onUnHighlighted;
    public Action onHighlighted;
    public Button button;
    public PartyMemberCombat partyMemberCombat;

    public virtual void OnSelect(BaseEventData eventData)
    {
        onHighlighted.Invoke();
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        onUnHighlighted.Invoke();
    }
} 
