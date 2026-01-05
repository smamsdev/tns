using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonHighlighted : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Action onHighlighed;
    public Action onUnHighlighed;
    public Button button;

    public void OnSelect(BaseEventData eventData)
    {
        onHighlighed?.Invoke();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        onUnHighlighed?.Invoke();
    }

    public void ButtonSelectedAndDisabled()
    {
        SetButtonNormalColor(Color.yellow);
        onHighlighed = null;
        onUnHighlighed = null;
    }

    public void SetButtonNormalColor(Color color)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        button.colors = colors;
    }
}
