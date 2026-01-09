using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonHighlighted : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Action onHighlighted;
    public Action onUnHighlighted;
    public Button button;

    public void OnSelect(BaseEventData eventData)
    {
        onHighlighted?.Invoke();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        onUnHighlighted?.Invoke();
    }

    public void ButtonSelectedAndDisabled()
    {
        SetButtonNormalColor(Color.yellow);
        onHighlighted = null;
        onUnHighlighted = null;
    }

    public void SetButtonNormalColor(Color color)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        button.colors = colors;
    }
}
