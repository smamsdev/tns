using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuManager : MonoBehaviour
{
    [Header("MENUS")]
    public ShopMainMenu mainMenu;
    public ShopBuyMenu buyMenu;
    public ShopSellMenu sellMenu;
    public ShopExitMenu exitMenu;

    [Header("Debug")]
    public ShopMenu menuUpdateMethod;
    public ShopMenu menuToDisplay;

    public void OpenShop()
    {
        mainMenu.InitialiseShop();
        menuUpdateMethod = mainMenu;
        mainMenu.DisplayMenu(true);
        mainMenu.EnterMenu();
    }

    public void DisplaySubMenu(ShopMenu shopMenuToDisplay)
    {
        buyMenu.DisplayMenu(false);
        sellMenu.DisplayMenu(false);
        exitMenu.DisplayMenu(false);

        shopMenuToDisplay.DisplayMenu(true);
        menuToDisplay = shopMenuToDisplay;
    }

    public void EnterMenu(ShopMenu shopMenu)
    {
        menuUpdateMethod = shopMenu;
        shopMenu.EnterMenu();
    }

    void Update()
    {
        StateUpdate(menuUpdateMethod);
    }

    void StateUpdate(ShopMenu menuUpdateMethod)
    {
        menuUpdateMethod.StateUpdate();
    }


}
