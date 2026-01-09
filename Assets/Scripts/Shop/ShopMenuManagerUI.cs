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

        buyMenu.InstantiateUIShopInventorySlots();
        //sellMenu.InstantiateUIInventorySlots();
        WireAllMainButtons();

        DisplayMenu(mainMenu);
        menuUpdateMethod = mainMenu;
        mainShopMenuButtons[0].onHighlighted();
    }

    void WireAllMainButtons()
    {
        WireButtonHighlightContainerDisplays(mainShopMenuButtons[0], mainShopDisplayContainers[0]);
        WireButtonHighlightContainerDisplays(mainShopMenuButtons[1], mainShopDisplayContainers[1]);
        WireButtonHighlightContainerDisplays(mainShopMenuButtons[2], mainShopDisplayContainers[2]);
    }

    void WireButtonHighlightContainerDisplays(MenuButtonHighlighted button, GameObject container)
    {
        button.onHighlighted = () => container.SetActive(true);
        button.onUnHighlighted = () => container.SetActive(false);

        container.SetActive(false);
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
