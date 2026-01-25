using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChargingSlotMenu : ChargingMenu
{
    public InventorySlotUI[] chargingSlotUIs;
    public ChargingMainMenu chargingMainMenu;
    public TextMeshProUGUI pageHeaderTMP;

    public GameObject propertiesDisplay;

    public TextMeshProUGUI feeTMP, gearDescriptionTMP, durationDisplayTMP, smamsInventoryTMP;

    public int slotSelectedIndex;
    public bool isEquipping;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public override void EnterMenu()
    {
        chargingMainMenu.menuButtonHighlighteds[0].SetButtonNormalColor(Color.yellow);
        chargingMainMenu.menuButtonHighlighteds[0].button.interactable = false;

        propertiesDisplay.SetActive(true);

        chargingSlotUIs[0].button.Select();
    }

    public void InitialiseChargingSlots()
    {
        ClearText();

        var chargingSlots = chargingMainMenu.chargerSO.chargingSlots;

        for (int i = 0; i < chargingSlots.Length; i++)
        {
            InventorySlotUI slotUI = chargingSlotUIs[i];

            slotUI.onHighlighted = () =>
            {
                ChargingSlotHighlighted(slotUI);
                chargingMainMenu.SetEquipSlotColor(slotUI, Color.yellow);
            };

            slotUI.onUnHighlighted = () =>
            {
                chargingMainMenu.SetEquipSlotColor(slotUI, Color.white);
            };

            if (chargingSlots[i].gearSO == null)
            {
                slotUI.itemNameTMP.text = "CHARGING SLOT " + (i + 1) + ": EMPTY";
            }
            else
            {
                slotUI.itemNameTMP.text = chargingSlots[i].gearSO.name;
                slotUI.itemQuantityTMP.text = ": " + chargingSlots[i].charge.ToString() + "%";
            }

            slotUI.gearInstance = chargingSlots[i];
        }

        UpdateSmamsUI();
    }

    void ClearText()
    {
        feeTMP.text = "";
        gearDescriptionTMP.text = "";
        durationDisplayTMP.text = "";
        smamsInventoryTMP.text = "";
    }

    void ChargingSlotHighlighted(InventorySlotUI inventorySlotUI)
    {
        //.ToString("N0") + " $MAMS";

        if (inventorySlotUI.gearInstance.gearSO == null)
            gearDescriptionTMP.text =
                $"SELECT to choose Equipment\n" +
                $"Charging Fee: {chargingMainMenu.chargerSO.costPerCharge} $MAMS per charge";

        else
            gearDescriptionTMP.text = $"Description: {inventorySlotUI.gearInstance.gearSO.gearDescription}";
    }

    public void ChargingSlotSelected(InventorySlotUI chargingSlotSelected)
    {
        slotSelectedIndex = Array.IndexOf(chargingSlotUIs, chargingSlotSelected);

        if (isEquipping)
        {
            var gearInstanceInventory = chargingMainMenu.playerInventory.inventorySO.gearInstanceInventory;
            EquipmentInstance gearInstanceToCharge = chargingMenuManager.chargingEquipmentSelectMenu.equipmentInstanceToCharge;
            var chargingSlots = chargingMenuManager.chargingMainMenu.chargerSO.chargingSlots;

            chargingSlots[slotSelectedIndex] = gearInstanceToCharge;
            gearInstanceInventory.Remove(gearInstanceToCharge);

            chargingMenuManager.ChargingSlotSelectMenu.InitialiseChargingSlots();
            chargingMenuManager.chargingEquipmentSelectMenu.InitialiseInventoryUI();
            isEquipping = false;
        }

        else 
        {
            ExitMenu();
            chargingMenuManager.DisplaySubMenu(chargingMenuManager.chargingEquipmentSelectMenu);
            chargingMenuManager.EnterMenu(chargingMenuManager.chargingEquipmentSelectMenu);
        }
    }

    public void UpdateSmamsUI()
    {
        smamsInventoryTMP.text = "$MAMS: " + chargingMainMenu.playerPermanentStats.Smams.ToString("N0");
    }

    public override void ExitMenu()
    {
        ClearText();

        pageHeaderTMP.text = "Select Charging Slot:";

        chargingMainMenu.menuButtonHighlighteds[0].button.interactable = true;
        chargingMainMenu.menuButtonHighlighteds[0].SetButtonNormalColor(Color.white);
        chargingMenuManager.chargingMainMenu.firstButtonToSelect = chargingMenuManager.chargingMainMenu.menuButtonHighlighteds[0].button;

        chargingMenuManager.EnterMenu(chargingMenuManager.chargingMainMenu);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time);
        string playTimeDuration = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

        durationDisplayTMP.text = playTimeDuration;
    }
}
