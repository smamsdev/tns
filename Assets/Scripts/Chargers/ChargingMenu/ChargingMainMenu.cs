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

    public TextMeshProUGUI smamsInventoryTMP;
    public TextMeshProUGUI durationDisplayTMP;
    bool isMenuOn;

    public void InitializeChargingStationMenu()
    {
        firstButtonToSelect = menuButtonHighlighteds[0].button;
        isMenuOn = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponentInChildren<PlayerInventory>();
        playerPermanentStats = player.GetComponentInChildren<PlayerCombat>().playerPermanentStats;

        WireMainButtons();

        animator.enabled = true;
        animator.Play("OpenShop");

        FieldEvents.menuAvailable = false;
        CombatEvents.LockPlayerMovement();

        //instaiotn slots
        chargingMenuManager.ChargingSlotSelectMenu.InitialiseEquipSlots();
        //shopMenuManager.buyMenu.InstantiateUIShopInventorySlots();
        //shopMenuManager.sellMenu.InstantiateUIInventorySlots();
        UpdateSmamsUI();
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
            chargingMenuManager.DisplaySubMenu(chargingMenuManager.chargingExitMenu);
        };
    }

    public void SetEquipSlotColor(InventorySlotUI inventorySlot, Color normalColor)
    {
        inventorySlot.itemNameTMP.color = normalColor;
        inventorySlot.itemQuantityTMP.color = normalColor;
    }

    public void UpdateSmamsUI()
    {
        smamsInventoryTMP.text = playerPermanentStats.Smams.ToString("N0");
    }

    public override void DisplayMenu(bool on)
    {
        //
    }

    public override void EnterMenu()
    {
        firstButtonToSelect.Select();
    }

    public override void ExitMenu()
    {
        throw new System.NotImplementedException();
    }

    public override void StateUpdate()
    {
        if (isMenuOn)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time);
            string playTimeDuration = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

            durationDisplayTMP.text = playTimeDuration;
        }
    }
}
