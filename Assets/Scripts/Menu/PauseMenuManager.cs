using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public Menu statsPage, gearPageSelection, gearEquipSubPage, gearInventorySubPage, movesPage, moveInventory, configPage, savePage, exitPage, main;
    [Header("")]
    public Menu menuUpdateMethod;
    public GameObject[] subPageGOs;

    private void Start()
    {
        menuUpdateMethod = main;
        foreach (GameObject go in subPageGOs) 
        { 
            go.SetActive(true);
        }
    }

    public void ClearThenDisplayMenu (Menu menuScript)
    {
        main.DisplayMenu(false);
        statsPage.DisplayMenu(false);
        gearPageSelection.DisplayMenu(false);
        gearEquipSubPage.DisplayMenu(false);
        gearInventorySubPage.DisplayMenu(false);

        movesPage.DisplayMenu(false);
        moveInventory.DisplayMenu(false);
        //configPage.DisplayMenu(false);
        savePage.DisplayMenu(false);
        exitPage.DisplayMenu(false);

        menuScript.DisplayMenu(true);
    }

    public void SetLastButtonSelected(MenuButtonHighlighted menuButton)
    {
        menuUpdateMethod.lastParentButtonSelected = menuButton;
    }

    public void EnterMenu(Menu menuScript)
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
