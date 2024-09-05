using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IMenuButtonHighlighted : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Menu menuToDisplay;
    [SerializeField] GameObject blueUnderLine;
    public Color highlightedColor;
    public Button button;

    [SerializeField] MenuManagerUI menuManager;

    private void OnEnable()
    {
        blueUnderLine.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        blueUnderLine.SetActive(true);
        menuManager.DisplayMenu(menuToDisplay);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        blueUnderLine.SetActive(false);
    }

    public void SetButtonColor(Color color)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        button.colors = colors;
    }
}