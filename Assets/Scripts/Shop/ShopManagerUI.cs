using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManagerUI : MonoBehaviour
{
    [Header("MENUS")]
    public ShopMenu buy;
    public ShopMenu sell;
    public ShopMenu exit;
    public ShopMenu main;
    [Header("")]
    public ShopMenu menuUpdateMethod;
    public ShopMenu menuToDisplay;

    private void Start()
    {
        menuUpdateMethod = main;
    }

    public void DisplayMenu(ShopMenu shopMenu)
    {
        buy.DisplayMenu(false);
        sell.DisplayMenu(false);
        //exit.DisplayMenu(false);
        shopMenu.DisplayMenu(true);
        menuToDisplay = shopMenu;
    }

    public void EnterSubMenu(ShopMenu shopMenu)
    {
        shopMenu.EnterMenu();
        menuUpdateMethod = shopMenu;
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
