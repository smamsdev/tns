using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;

public class StyleSelectMenuUI : CombatMenu
{
    private void Start()
    {
        InitMenu();
    }

    public void InitMenu()
    {
        foreach (Button button in menuButtons)
        {
            MenuButtonHighlighted menuButtonHighlighted = button.AddComponent<MenuButtonHighlighted>();
            menuButtonHighlighted.tmp = button.GetComponentInChildren<TextMeshProUGUI>();
            menuButtonHighlighted.button = button;

            menuButtonHighlighted.onHighlighted = () => MenuOptionHighlighted(button);
        }
    }

    void MenuOptionHighlighted(Button button)
    {
        highlightedButtonIndex = menuButtons.IndexOf(button);
        UpdateNarrator(highlightedButtonIndex);
    }

    void UpdateNarrator(int index)
    {
        string text;

        switch (index)
        {
            case 0:
                text = "Execute a Violent Move?";
                break;
            case 1:
                text = "Execute a Cautious Move?";
                break;
            case 2:
                text = "Execute a Precise Move?";
                break;
            case 3:
                text = "Select Tactic?";
                break;
            default:
                Debug.Log("something went wrong");
                text = null;
                break;
        }

        menuManager.UpdateNarrator(text);   
    }
}
