using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerUI : MonoBehaviour
{
    public Menu statsPage, gearPage, movesPage, configPage, savePage, exitPage, main;
    [Header("")]
    public Menu menuUpdateMethod;

    private void Start()
    {
        menuUpdateMethod = main;
    }

    public void DisplayMenu(Menu menuScript)
    {
        statsPage.DisplayMenu(false);
        gearPage.DisplayMenu(false);
        movesPage.DisplayMenu(false);
        //configPage.DisplayMenu(false);
        //savePage.DisplayMenu(false);
        //exitPage.DisplayMenu(false);
        menuScript.DisplayMenu(true);
    }

    public void EnterSubMenu(Menu menuScript)
    {
        menuScript.EnterMenu();
        menuUpdateMethod = menuScript;
    }

    void Update()
    {
        StateUpdate(menuUpdateMethod);
    }

    void StateUpdate(Menu menuUpdateMethod)
    {
        menuUpdateMethod.StateUpdate();
    }
}
