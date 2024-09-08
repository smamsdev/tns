using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuSlotSelect : Menu
{
    public Button firstButtonToHighlight;
    public Color colourForSelectedParent;
    public MenuMoveType menuMoveTypeScript;

    public override void DisplayMenu(bool on)
    {
        throw new System.NotImplementedException();
    }

    public override void EnterMenu()
    {
        firstButtonToHighlight.Select();
        displayContainer.SetActive(true);

        ColorBlock colors = mainButtonToRevert.colors;
        colors.normalColor = colourForSelectedParent;
        mainButtonToRevert.colors = colors;
    }

    public override void ExitMenu()
    {
        ColorBlock colors = mainButtonToRevert.colors;
        colors.normalColor = Color.white;
        mainButtonToRevert.colors = colors;

        mainButtonToRevert.Select();

        menuManagerUI.menuUpdateMethod = menuMoveTypeScript;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
}
