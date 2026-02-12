using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuExit : PauseMenu
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
        pauseMenuManager.menuUpdateMethod = pauseMenuManager.main;
    }

    public override void StateUpdate()
    {
        throw new System.NotImplementedException();
    }

}
