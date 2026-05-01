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
    public TextMeshProUGUI headerTMP, shopnameTMP, descriptionFieldTMP, quantityTMP, valueTMP, smamsInventoryTMP, equipStatusTMP;
    public Button firstMenuButton;
    public Animator animator, smamsColorAnimator;
    public PlayerInventorySO playerInventorySO;
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
        DescriptionFieldClear();
        displayContainer.SetActive(true);
        this.gameObject.SetActive(true);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerInventorySO = player.GetComponent<PlayerCombat>().playerInventorySO;
        playerPermanentStats = player.GetComponent<PlayerCombat>().playerPermanentStats;

        animator.Play("OpenMenu");

        FieldEvents.menuAvailable = false;
        CombatEvents.LockPlayerMovement();

        shopMenuManager.buyMenu.InstantiateUIShopInventorySlots();
        shopMenuManager.sellMenu.InitialiseInventoryUI();
        UpdateSmamsUI();
    }

    IEnumerator OpenMenuAnimation()
    {
        yield return null;
        animator.Play("OpenMenu");
    }

    public void DisplayMainButtons(bool on)
    {
        foreach (MenuButtonHighlighted menuButtonHighlighted in mainShopMenuButtons)
        {
            menuButtonHighlighted.gameObject.SetActive(on);
        }
    }

    public override void EnterMenu()
    {
        WireAllMainButtons();
        firstMenuButton.Select();
    }

    public void UpdateSmamsUI()
    {
        smamsInventoryTMP.text = playerPermanentStats.SmamsFormattedString();
    }

    public override void ExitMenu()
    {
        animator.Play("CloseMenu", 0, 0f);
        FieldEvents.menuAvailable = true;
        CombatEvents.UnlockPlayerMovement();
        //Manager GO will be disabled via attached MenuAnimationFunctions script event once completed
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ExitMenu();
    }

    public void SetHeaderTMP(string text)
    {
        headerTMP.text = text;
    }

    public void DescriptionFieldClear()
    {
        quantityTMP.text = "";
        descriptionFieldTMP.text = "";
        valueTMP.text = "";
        equipStatusTMP.text = "";
    }

    public void UpdateDescriptionField(GearInstance gearInstance)
    {
        descriptionFieldTMP.text = gearInstance.DescriptionFormatted();

        if (shopMenuManager.menuUpdateMethod == shopMenuManager.buyMenu)
        { 
            valueTMP.text = gearInstance.BuyValueFormattedString(shopMenuManager.buyMenu.shop.shopMarkupPer);
            quantityTMP.text = "";
            equipStatusTMP.text = "";
            return;
        }

        if (shopMenuManager.menuUpdateMethod == shopMenuManager.sellMenu)
        {
            valueTMP.text = gearInstance.SellValueFormattedString();

            if (gearInstance.isCurrentlyEquipped)
                equipStatusTMP.text = "Equipped to Slot " + (gearInstance.EquippedSlotInt(playerInventorySO) + 1) + ". Press CTRL to unequip";

            else
                equipStatusTMP.text = "";

            if (gearInstance is EquipmentInstance equipmentInstance)
            {
                quantityTMP.text = equipmentInstance.ChargeTotalString();
                return;
            }

            quantityTMP.text = gearInstance.QuantityString();
        }
    }
}
