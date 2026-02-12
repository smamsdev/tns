using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopMainMenu : ShopMenu
{
    [Header("")]

    public TextMeshProUGUI shopnameTMP;
    public TextMeshProUGUI smamsInventoryTMP;
    public Button firstMenuButton;
    public Animator animator;
    public Animator smamsColorAnimator;

    public GameObject GearDescriptionGO;

    [SerializeField] TextMeshProUGUI descriptionFieldTMP;
    [SerializeField] TextMeshProUGUI itemTypeTMP;
    [SerializeField] TextMeshProUGUI itemValueTMP;

    public GameObject player;
    public PlayerInventory playerInventory;
    public PlayerPermanentStats playerPermanentStats;

    public MenuButtonHighlighted[] mainShopMenuButtons;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(true);
    }

    public void WireAllMainButtons()
    {
        WireButtonHighlightContainerDisplays(mainShopMenuButtons[0], shopMenuManager.buyMenu);
        WireButtonHighlightContainerDisplays(mainShopMenuButtons[1], shopMenuManager.sellMenu);
    }

    void WireButtonHighlightContainerDisplays(MenuButtonHighlighted button, ShopMenu shopMenuToDisplay)
    {
        button.onHighlighted = () => shopMenuManager.DisplaySubMenu(shopMenuToDisplay);
        button.onUnHighlighted = () => { };
    }

    public void InitialiseShop()
    {
        displayContainer.SetActive(true);
        this.gameObject.SetActive(true);

        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponentInChildren<PlayerInventory>();
        playerPermanentStats = player.GetComponentInChildren<PlayerCombat>().playerPermanentStats;

        animator.Play("OpenShop");

        FieldEvents.menuAvailable = false;
        CombatEvents.LockPlayerMovement();

        shopMenuManager.buyMenu.InstantiateUIShopInventorySlots();
        shopMenuManager.sellMenu.InstantiateUIInventorySlots();
        UpdateSmamsUI();
    }

    IEnumerator OpenShopAnimation()
    {
        yield return null;
        animator.Play("OpenShop");
    }

    public void CloseShop()
    {
        animator.Play("CloseShop", 0, 0f);
        FieldEvents.menuAvailable = true;
        CombatEvents.UnlockPlayerMovement();
        //Manager GO will be disabled via animation event once completed
    }


    public override void EnterMenu()
    {
        WireAllMainButtons();

        shopMenuManager.mainMenu.GearDescriptionGO.SetActive(false);
        firstMenuButton.Select();
    }

    public void UpdateSmamsUI()
    {
        smamsInventoryTMP.text = playerPermanentStats.Smams.ToString("N0");
    }

    public override void ExitMenu()
    {
        animator.Play("CloseShop");
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            CloseShop();
        }
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

        if (shopMenuManager.menuUpdateMethod == shopMenuManager.buyMenu)
        {
            itemValueTMP.text = "Buy: " + gear.value.ToString("N0") + " $MAMS";
        }

        if (shopMenuManager.menuUpdateMethod == shopMenuManager.sellMenu)
        {
            itemValueTMP.text = "Sell: " + (gear.value / 2).ToString("N0") + " $MAMS";
        }
    }
}
