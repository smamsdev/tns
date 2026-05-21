using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class PartyMemberUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Action onUnHighlighted;
    public Action onHighlighted;
    public Button button;
    public TextMeshProUGUI nameTMP, levelTMP;
    public PartyMemberCombat partyMemberCombat;
    public PartyMemberPortrait partyMemberPortrait;

    public virtual void OnSelect(BaseEventData eventData)
    {
        onHighlighted?.Invoke();
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        onUnHighlighted?.Invoke();
    }
} 
