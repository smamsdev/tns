using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IShopButtonHighlighted : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    //delete

    [SerializeField] ShopMenu menuToDisplay;
    [SerializeField] GameObject blueUnderLine;
    public Color highlightedColor;
    public Button button;

    [SerializeField] ShopMenuManagerUI shopManagerUI;

    private void OnEnable()
    {
        blueUnderLine.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        blueUnderLine.SetActive(true);
        shopManagerUI.DisplayMenu(menuToDisplay);
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