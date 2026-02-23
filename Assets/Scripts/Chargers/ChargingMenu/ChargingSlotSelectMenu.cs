using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class ChargingSlotMenu : ChargingMenu
{
    public InventorySlotUI[] chargingSlotUIs;
    public Animator[] charingSlotTMPanimators;
    public ChargingMainMenu chargingMainMenu;
    public TextMeshProUGUI pageHeaderTMP;

    public GameObject propertiesDisplay;
    public float chargeTimer;
    public int rotatingChargerIndex = 0;

    public TextMeshProUGUI feeTMP, gearDescriptionTMP, gearTypeTMP, smamsInventoryTMP;

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
            chargingMainMenu.chargerSO.ApplyChargeToSlot(i);

            slotUI.onHighlighted = () =>
            {
                ChargingSlotHighlighted(slotUI);
                chargingMainMenu.SetEquipSlotColor(slotUI, Color.yellow);
            };

            slotUI.onUnHighlighted = () =>
            {
                chargingMainMenu.SetEquipSlotColor(slotUI, Color.white);
            };

            if (chargingSlots[i] == null || chargingSlots[i].gearSO == null)
            {
                slotUI.itemNameTMP.text = "CHARGING SLOT " + (i + 1) + ": EMPTY";
                slotUI.itemQuantityTMP.text = "";
            }
            else
            {
                slotUI.itemNameTMP.text = chargingSlots[i].gearSO.name;
                slotUI.itemQuantityTMP.text = ": " + chargingSlots[i].ChargePercentage() + "%";
            }

            slotUI.gearInstance = chargingSlots[i];
        }

        UpdateSmamsUI();
    }

    void ClearText()
    {
        feeTMP.text = "";
        gearDescriptionTMP.text = "";
    }

    void ChargingSlotHighlighted(InventorySlotUI inventorySlotUI)
    {
        slotSelectedIndex = Array.IndexOf(chargingSlotUIs, inventorySlotUI);

        if (inventorySlotUI.gearInstance == null || inventorySlotUI.gearInstance.gearSO == null)
        {
            feeTMP.text = $"Charging Fee: {chargingMainMenu.chargerSO.costPerCharge} $MAMS per charge";
            gearDescriptionTMP.text = $"SELECT to choose Equipment";
            gearTypeTMP.text = "";
        }

        else
        {
            int chargesAccrued = ((EquipmentInstance)inventorySlotUI.gearInstance).ChargesAccrued;
            int costPerCharge = chargingMainMenu.chargerSO.costPerCharge;

            feeTMP.text = "Charging Fee: " + (chargesAccrued * costPerCharge) + " $MAMS";
            gearDescriptionTMP.text = $"Description: {inventorySlotUI.gearInstance.gearSO.gearDescription}";

            EquipmentInstance equipmentInstance = inventorySlotUI.gearInstance as EquipmentInstance;
            EquipmentSO equipmentSO = equipmentInstance.gearSO as EquipmentSO;

            gearTypeTMP.text = "Charge " + equipmentInstance.Charge + " / " + equipmentSO.maxPotential;
        }
    }

    public void ChargingSlotSelected(InventorySlotUI chargingSlotSelected)
    {
        if (isEquipping)
        {
            if (chargingSlotSelected.gearInstance != null && chargingSlotSelected.gearInstance.gearSO != null)
                return;

            var gearInstanceInventory = chargingMainMenu.playerInventory.inventorySO.gearInstanceInventory;
            EquipmentInstance gearInstanceToCharge = chargingMenuManager.chargingEquipmentSelectMenu.equipmentInstanceToCharge;
            var chargingSlots = chargingMenuManager.chargingMainMenu.chargerSO.chargingSlots;

            chargingSlots[slotSelectedIndex] = gearInstanceToCharge;
            gearInstanceToCharge.StartCharging();
            gearInstanceInventory.Remove(gearInstanceToCharge);

            chargingMenuManager.ChargingSlotSelectMenu.InitialiseChargingSlots();
            chargingMenuManager.chargingEquipmentSelectMenu.InitialiseInventoryUI();

            pageHeaderTMP.text = "Select Charging Slot:";

            isEquipping = false;
            ChargingSlotHighlighted(chargingSlotUIs[slotSelectedIndex]);
        }

        else
        {
            if (chargingSlotSelected.gearInstance == null || chargingSlotSelected.gearInstance.gearSO == null)
            {
                ExitMenu();
                chargingMenuManager.DisplaySubMenu(chargingMenuManager.chargingEquipmentSelectMenu);
                chargingMenuManager.EnterMenu(chargingMenuManager.chargingEquipmentSelectMenu);
            }

            else
            {
                RetrieveGear(chargingSlotSelected);
            }
        }
    }

    void RetrieveGear(InventorySlotUI chargingSlotSelected)
    {
        int chargesAccrued = ((EquipmentInstance)chargingSlotSelected.gearInstance).ChargesAccrued;
        int costPerCharge = chargingMainMenu.chargerSO.costPerCharge;
        int fee = chargesAccrued * costPerCharge;
        var stats = chargingMainMenu.playerPermanentStats;

        if (fee > stats.Smams)
        {
            smamsInventoryTMP.GetComponent<Animator>().Play("SmamsRedText");
            return;
        }

        smamsInventoryTMP.GetComponent<Animator>().Play("SmamsRedText");

        StartCoroutine(FieldEvents.LerpValuesCoRo(stats.Smams, stats.Smams -= fee, .2f,
            smamsValue =>
            {
                smamsInventoryTMP.text = "$MAMS: " + Mathf.RoundToInt(smamsValue).ToString("N0");
            }));

        bool inventorySpaceAvailable = chargingMainMenu.playerInventory.inventorySO.AttemptAddGearToInventory(chargingSlotSelected.gearInstance);

        if (inventorySpaceAvailable)
        {
            chargingMainMenu.chargerSO.chargingSlots[slotSelectedIndex] = null;
            InitialiseChargingSlots();
            chargingMenuManager.chargingEquipmentSelectMenu.InitialiseInventoryUI();
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

    public void CheckCharges()
    {
        chargeTimer += Time.deltaTime;

        if (chargeTimer < .5f)
            return;

        // Preserve leftover time
        chargeTimer -= .5f;

        var slot = chargingSlotUIs[rotatingChargerIndex];

        if (slot.gearInstance != null && slot.gearInstance.gearSO != null)
        {
            if (((EquipmentInstance)slot.gearInstance).ChargePercentage() < 100)
            {
                chargingMainMenu.chargerSO.ApplyChargeToSlot(rotatingChargerIndex);
                slot.itemQuantityTMP.text = ": " + ((EquipmentInstance)slot.gearInstance).ChargePercentage() + "%";
                ChargingSlotHighlighted(chargingSlotUIs[slotSelectedIndex]);

                charingSlotTMPanimators[rotatingChargerIndex].Play("ChargingSlotTMPAnimation");
            }
        }

        rotatingChargerIndex = (rotatingChargerIndex + 1) % chargingSlotUIs.Length;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }

        CheckCharges();
    }
}
