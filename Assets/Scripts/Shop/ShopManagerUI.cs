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
    public ShopMainMenu mainMenu;

    [SerializeField] TextMeshProUGUI descriptionFieldTMP;
    [SerializeField] TextMeshProUGUI itemTypeTMP;
    [SerializeField] TextMeshProUGUI itemValueTMP;
    public Animator smamsColorAnimator;

    public PlayerInventory playerInventory;

    private void OnEnable()
    {
        menuUpdateMethod = main;

        var player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponentInChildren<PlayerInventory>();
        DisplayMenu(main);
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

    public void UpdateDescriptionField(Gear gear)
    {
        descriptionFieldTMP.text = gear.gearDescription;

        if (!gear.isConsumable)
        {
            itemTypeTMP.text = "Equipment";
        }

        else
        {
            itemTypeTMP.text = "Consumable";
        }

        if (menuToDisplay == buy)

        {
            itemValueTMP.text = "Price to buy: " + gear.value;
        }

        if (menuToDisplay == sell)

        {
            itemValueTMP.text = "Price to sell: " + gear.value / 2;
        }
    }
}
