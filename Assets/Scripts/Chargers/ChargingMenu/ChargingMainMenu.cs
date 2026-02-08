using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargingMainMenu : ChargingMenu
{
    public PlayerInventory playerInventory;
    public PlayerPermanentStats playerPermanentStats;
    public ChargerSO chargerSO;

    public Animator animator;
    public Button firstButtonToSelect;
    public MenuButtonHighlighted[] menuButtonHighlighteds;

    public void InitializeChargingStationMenu()
    {
        firstButtonToSelect = menuButtonHighlighteds[0].button;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponentInChildren<PlayerInventory>();
        playerPermanentStats = player.GetComponentInChildren<PlayerCombat>().playerPermanentStats;

        WireMainButtons();

        this.gameObject.SetActive(true);
        displayContainer.SetActive(true);
        animator.Play("OpenShop");

        FieldEvents.menuAvailable = false;
        CombatEvents.LockPlayerMovement();

        chargingMenuManager.ChargingSlotSelectMenu.rotatingChargerIndex = 0;
        chargingMenuManager.ChargingSlotSelectMenu.gameObject.SetActive(true);
        chargingMenuManager.chargingEquipmentSelectMenu.gameObject.SetActive(true);

        chargingMenuManager.ChargingSlotSelectMenu.InitialiseChargingSlots();
        chargingMenuManager.chargingEquipmentSelectMenu.InitialiseInventoryUI();

        EnterMenu();
    }

    void WireMainButtons()
    {
        menuButtonHighlighteds[0].onHighlighted = () => 
        {
            chargingMenuManager.DisplaySubMenu(chargingMenuManager.ChargingSlotSelectMenu);
        };

        menuButtonHighlighteds[1].onHighlighted = () =>
        {
            chargingMenuManager.DisplaySubMenu(chargingMenuManager.chargingEquipmentSelectMenu);
        };

        menuButtonHighlighteds[2].onHighlighted = () =>
        {
            chargingMenuManager.subMenuPanel.gameObject.SetActive(false);
        };

        menuButtonHighlighteds[2].onUnHighlighted = () =>
        {
            chargingMenuManager.subMenuPanel.gameObject.SetActive(true);
        };
    }

    public void SetEquipSlotColor(InventorySlotUI inventorySlot, Color normalColor)
    {
        inventorySlot.itemNameTMP.color = normalColor;
        inventorySlot.itemQuantityTMP.color = normalColor;
    }

    public override void DisplayMenu(bool on)
    {
        //
    }

    public override void EnterMenu()
    {
        firstButtonToSelect.Select();
        chargingMenuManager.ChargingSlotSelectMenu.propertiesDisplay.SetActive(false);
    }

    public override void ExitMenu()
    {
        chargingMenuManager.chargingEquipmentSelectMenu.DeleteAllInventoryUI();
        animator.Play("CloseShop"); //animator has a Function to disable this GO once finished
        CombatEvents.UnlockPlayerMovement();
        FieldEvents.menuAvailable = true;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }

        if (chargingMenuManager.ChargingSlotSelectMenu.displayContainer.activeInHierarchy)
        chargingMenuManager.ChargingSlotSelectMenu.CheckCharges();
    }
}
