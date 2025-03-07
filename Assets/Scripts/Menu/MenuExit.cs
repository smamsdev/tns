using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuExit : Menu
{
    public GameObject exitToMainMenuGo;
    public Button yes, no;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public override void EnterMenu()
    {
        exitToMainMenuGo.SetActive(true);
        no.Select();
    }

    public void ConfirmExit()

    {
        Debug.Log("no implemented");
    }

    public void DeclineExit()

    {
        exitToMainMenuGo.SetActive(false);
        ExitMenu();
    }

    public override void ExitMenu()
    {
        menuButtonHighlighted.SetButtonColor(Color.white);
        menuButtonHighlighted.enabled = true; //this keeps the blue underline
        mainButtonToRevert.Select();
        menuManagerUI.menuUpdateMethod = menuManagerUI.main;
    }

    public override void StateUpdate()
    {
        throw new System.NotImplementedException();
    }

}
