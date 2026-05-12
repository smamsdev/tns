using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatMenu : MonoBehaviour
{
    public CombatMenuManager menuManager;
    public List<Button> menuButtons;
    public int highlightedButtonIndex;

    public void DisplayMenu(bool on)
    {
        this.gameObject.SetActive(on);
    }

    public void ButtonHighlighted(Button button)
    {
        highlightedButtonIndex = menuButtons.IndexOf(button);
    }

    public void SetButtonNormalColor(Button button, Color color)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        button.colors = colors;
    }
}
