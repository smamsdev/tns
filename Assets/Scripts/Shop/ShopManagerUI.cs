using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TextMeshProUGUI descriptionFieldTMP;
    public TextMeshProUGUI itemTypeTMP;

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

    public void UpdateDescriptionField(string text, bool isEquipment)
    {
        descriptionFieldTMP.text = text;

        if (isEquipment)
        {
            itemTypeTMP.text = "Equipment";
        }

        else
        {
            itemTypeTMP.text = "Consumable";
        }
    }
}
