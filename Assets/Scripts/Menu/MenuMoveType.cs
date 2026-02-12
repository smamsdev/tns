using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuMoveType : PauseMenu
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
//
    }

    public override void EnterMenu()
    {
        displayContainer.SetActive(true);
        firstButtonToHighlight.Select();

        parentSymbolImage.SetActive(true);
    }

    public override void ExitMenu()
    {
        displayContainer.SetActive(false);


        pauseMenuManager.menuUpdateMethod = pauseMenuManager.movesPage;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
}
