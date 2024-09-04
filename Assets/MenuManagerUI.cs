using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuManagerUI : MonoBehaviour
{
    public Menu statsPage, gearPage, movesPage, configPage, savePage, exitPage;

    public void SelectMenu(Menu menuScript)
    {
        menuScript.EnterMenu();
    }

    public void DisplayMenu(Menu menuScript)
    {
        statsPage.displayContainer.SetActive(false);
        gearPage.displayContainer.SetActive(false);
        //movesPage.displayContainer.SetActive(false);
        //configPage.displayContainer.SetActive(false);
        //savePage.displayContainer.SetActive(false);
        //exitPage.displayContainer.SetActive(false);

        menuScript.displayContainer.SetActive(true);
    }

}
