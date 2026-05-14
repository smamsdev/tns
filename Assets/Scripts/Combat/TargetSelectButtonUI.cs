using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TargetSelectButtonUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Action onHighlighted;
    public Action onUnHighlighted;
    public TextMeshProUGUI tmp;
    public Button button;
    public Combatant combatant;

    public void OnSelect(BaseEventData eventData)
    {
        onHighlighted?.Invoke();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        onUnHighlighted?.Invoke();
    }
}
