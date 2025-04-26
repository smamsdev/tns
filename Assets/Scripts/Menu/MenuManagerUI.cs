using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerUI : MonoBehaviour
{
    public Menu statsPage, gearPage, gearEquipPage, movesPage, configPage, savePage, exitPage, main;
    [Header("")]
    public Menu menuUpdateMethod;

    private void Start()
    {
        menuUpdateMethod = main;
    }

    public void DisplayMenu(Menu menuScript) //used by main buttons onSelect to display currently highlighted
    {
        statsPage.DisplayMenu(false);
        gearPage.DisplayMenu(false);
        movesPage.DisplayMenu(false);
        gearEquipPage.DisplayMenu(false);
        //configPage.DisplayMenu(false);
        savePage.DisplayMenu(false);
        exitPage.DisplayMenu(false);
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

    public void SetTextAlpha(TextMeshProUGUI textMeshProUGUI, float alpha) //other classes to want to use this a bunch so put it here
    {
        Color color = textMeshProUGUI.color;
        color.a = alpha;
        textMeshProUGUI.color = color;
    }
}
