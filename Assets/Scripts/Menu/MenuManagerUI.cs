using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerUI : MonoBehaviour
{
    public Menu statsPage, gearPage, movesPage, configPage, savePage, exitPage, main;
    [Header("")]
    public Menu menuUpdateMethod;
    public TextMeshProUGUI descriptionFieldTMP;
    public TextMeshProUGUI itemTypeTMP;
    public TextMeshProUGUI itemvalue;

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

    public void UpdateDescriptionField(string text, bool isEquipment)
    {
        descriptionFieldTMP.text = text;

        if (!isEquipment)
        {
            itemTypeTMP.text = "Equipment";
        }

        else
        {
            itemTypeTMP.text = "Consumable";
        }

    }
}
