using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuManagerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descriptionFieldTMP;
    [SerializeField] TextMeshProUGUI itemTypeTMP;
    [SerializeField] TextMeshProUGUI itemValueTMP;

    public GameObject GearDescriptionGO;

    public Animator smamsColorAnimator;
    public MenuButtonHighlighted[] mainShopMenuButtons;
    public GameObject[] mainShopDisplayContainers;

    public GameObject player;
    public PlayerInventory playerInventory;
    public PlayerPermanentStats playerPermanentStats;

    [Header("MENUS")]
    public ShopMainMenu mainMenu;
    public ShopBuyMenu buyMenu;
    public ShopSellMenu sellMenu;
    public ShopExitMenu exitMenu;

    [Header("Debug")]
    public ShopMenu menuUpdateMethod;
    public ShopMenu menuToDisplay;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponentInChildren<PlayerInventory>();
        playerPermanentStats = player.GetComponentInChildren<PlayerCombat>().playerPermanentStats;

        buyMenu.InstantiateUIShopInventorySlots();
        sellMenu.InstantiateUIInventorySlots();
        WireAllMainButtons();
        mainMenu.OpenShop();
        DisplayMenu(mainMenu);
        EnterSubMenu(mainMenu);
        mainShopMenuButtons[0].onHighlighted();
    }

    public void WireAllMainButtons()
    {
        WireButtonHighlightContainerDisplays(mainShopMenuButtons[0], buyMenu);
        WireButtonHighlightContainerDisplays(mainShopMenuButtons[1], sellMenu);
        WireButtonHighlightContainerDisplays(mainShopMenuButtons[2], exitMenu);
    }

    void WireButtonHighlightContainerDisplays(MenuButtonHighlighted button, ShopMenu shopMenuToDisplay)
    {
        button.onHighlighted = () => DisplayMenu(shopMenuToDisplay);
        button.onUnHighlighted = () => { };
    }

    public void DisplayMenu(ShopMenu shopMenuToDisplay)
    {
        buyMenu.DisplayMenu(false);
        sellMenu.DisplayMenu(false);
        exitMenu.DisplayMenu(false);

        shopMenuToDisplay.DisplayMenu(true);
        menuToDisplay = shopMenuToDisplay;
    }

    public void EnterSubMenu(ShopMenu shopMenu)
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

    public void UpdateDescriptionField(GearSO gear)
    {
        descriptionFieldTMP.text = gear.gearDescription;

        if (gear is EquipmentSO)
        {
            itemTypeTMP.text = "Type: Equipment";
        }

        else
        {
            itemTypeTMP.text = "Type: Consumable";
        }

        if (menuUpdateMethod == buyMenu)

        {
            itemValueTMP.text = "$MAMS to buy: " + gear.value;
        }

        if (menuUpdateMethod == sellMenu)

        {
            itemValueTMP.text = "$MAMS to sell: " + gear.value / 2;
        }
    }
}
