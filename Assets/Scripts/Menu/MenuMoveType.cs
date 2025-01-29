using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuMoveType : Menu
{
    public Button firstButtonToHighlight;
    public Color colourForSelectedParent;
    public GameObject parentSymbolImage;

    private void Start()
    {
        displayContainer.SetActive(false);
    }

    public override void DisplayMenu(bool on)
    {
        throw new System.NotImplementedException();
    }

    public override void EnterMenu()
    {
        displayContainer.SetActive(true);
        firstButtonToHighlight.Select();

        ColorBlock colors = mainButtonToRevert.colors;
        colors.normalColor = colourForSelectedParent;
        mainButtonToRevert.colors = colors;
        parentSymbolImage.SetActive(true);
    }

    public override void ExitMenu()
    {
        displayContainer.SetActive(false);

        ColorBlock colors = mainButtonToRevert.colors;
        colors.normalColor = Color.white;
        mainButtonToRevert.colors = colors;

        mainButtonToRevert.Select();
        menuManagerUI.menuUpdateMethod = menuManagerUI.movesPage;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
}
