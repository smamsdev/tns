using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class ChargingSlotMenu : ChargingMenu
{
    public List<InventorySlotUI> chargingSlotUIs;
    public List<Animator> charingSlotTMPanimators;
    public List<Button> chargingSlotButtons;
    public ChargingMainMenu chargingMainMenu;
    public TextMeshProUGUI pageHeaderTMP;
    public GameObject ChargingSlotUIsParent, chargingSlotPrefab;

    public float chargeTimer;
    public int rotatingChargerIndex = 0;

    public TextMeshProUGUI feeTMP, gearDescriptionTMP, chargeAmountTMP, smamsInventoryTMP, headerTMP;

    public int slotSelectedIndex;
    public bool isEquipping;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
        headerTMP.text = "Select charging slot:";
        feeTMP.text = $"{chargingMainMenu.chargerSO.costPerCharge} $MAMS per charge";
        chargeAmountTMP.text = "";
        gearDescriptionTMP.text = "";
    }

    public override void EnterMenu()
    {
        chargingMainMenu.menuButtonHighlighteds[0].SetButtonNormalColor(Color.yellow);
        chargingMainMenu.menuButtonHighlighteds[0].button.interactable = false;

        chargingSlotUIs[0].button.Select();
    }

    public void DeleteAllInventoryUI()
    {
        chargingSlotUIs.Clear();

        for (int i = ChargingSlotUIsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(ChargingSlotUIsParent.transform.GetChild(i).gameObject);
        }
    }

    public void InstantiateAllChargingSlots()
    {
        var chargingSlots = chargingMainMenu.chargerSO.chargingSlots;
        chargingSlotUIs = new List <InventorySlotUI>();
        charingSlotTMPanimators = new List<Animator>();
        chargingSlotButtons = new List<Button>();

        ClearText();
        DeleteAllInventoryUI();

        for (int i = 0; i < chargingSlots.Length; i++)
        {
            InstantiateEmptySlotUIGO(i);
            SyncSlotFromSO(chargingSlotUIs[i]);
        }

        FieldEvents.SetGridNavigationWrapAround(chargingSlotButtons, chargingSlots.Length);
        UpdateSmamsUI();
    }

    void InstantiateEmptySlotUIGO(int slotIndex)
    {
        GameObject ChargingSlotGO = Instantiate(chargingSlotPrefab, ChargingSlotUIsParent.transform);
        InventorySlotUI inventorySlotUI = ChargingSlotGO.GetComponent<InventorySlotUI>();

        ChargingSlotGO.name = "Charging Slot " + slotIndex;

        inventorySlotUI.button.onClick.AddListener(() => ChargingSlotSelected(inventorySlotUI));

        inventorySlotUI.onHighlighted = () =>
        {
            ChargingSlotHighlighted(inventorySlotUI);
            UpdateHeader(inventorySlotUI);
            UpdateEquipmentChargeDisplay(inventorySlotUI);
            chargingMainMenu.SetEquipSlotColor(inventorySlotUI, Color.yellow);
        };

        inventorySlotUI.onUnHighlighted = () =>
        {
            chargingMainMenu.SetEquipSlotColor(inventorySlotUI, Color.white);
        };

        chargingSlotUIs.Add(inventorySlotUI);
        charingSlotTMPanimators.Add(inventorySlotUI.GetComponent<Animator>());
        chargingSlotButtons.Add(inventorySlotUI.button);
    }

    void SyncSlotFromSO(InventorySlotUI inventorySlotUI)
    {
        int index = chargingSlotUIs.IndexOf(inventorySlotUI);
        var slot = chargingMainMenu.chargerSO.chargingSlots[index];

        if (slot.gearSO == null)
        {
            inventorySlotUI.gearInstance = new EquipmentInstance();
            inventorySlotUI.itemNameTMP.text = $"Charging Slot {index + 1}: Empty";
            inventorySlotUI.itemQuantityTMP.text = "";
            inventorySlotUI.icon.sprite = inventorySlotUI.freeIcon;
            return;
        }

        inventorySlotUI.gearInstance = slot;
        inventorySlotUI.itemNameTMP.text = slot.gearSO.name;
        inventorySlotUI.itemQuantityTMP.text = $": {slot.ChargePercentage()}%";
        inventorySlotUI.icon.sprite = inventorySlotUI.equipmentIcon;
    }

    void ClearText()
    {
        feeTMP.text = "";
        gearDescriptionTMP.text = "";
    }

    void ChargingSlotHighlighted(InventorySlotUI inventorySlotUI)
    {
        slotSelectedIndex = chargingSlotUIs.IndexOf(inventorySlotUI);

        if (inventorySlotUI.gearInstance.gearSO == null)
        {
            chargeAmountTMP.text = "";
            gearDescriptionTMP.text = "";
        }

        else
        {
            gearDescriptionTMP.text = $"Description: {inventorySlotUI.gearInstance.gearSO.gearDescription}";
        }
    }

    void UpdateHeader(InventorySlotUI inventorySlotUI)
    {
        if (isEquipping && (inventorySlotUI.gearInstance.gearSO == null))
        {
            EquipmentInstance gearInstanceToCharge = chargingMenuManager.chargingEquipmentSelectMenu.equipmentInstanceToCharge;

            headerTMP.text = "Charge " + gearInstanceToCharge.gearSO.gearName + " in slot " + (slotSelectedIndex+1) + "?";
            return;
        }

        if (inventorySlotUI.gearInstance.gearSO == null)
        {
            headerTMP.text = $"Select EQUIPMENT to charge";
            return;
        }

        EquipmentInstance equipmentInstance = inventorySlotUI.gearInstance as EquipmentInstance;

        headerTMP.text = $"Retrieve {equipmentInstance.gearSO.name}?";
    }

    void UpdateEquipmentChargeDisplay(InventorySlotUI inventorySlotUI)
    {
        if (inventorySlotUI.gearInstance.gearSO == null)
        {
            chargeAmountTMP.text = "";
            feeTMP.text = $"{chargingMainMenu.chargerSO.costPerCharge} $MAMS per charge";
            return;
        }

        int chargesAccrued = ((EquipmentInstance)inventorySlotUI.gearInstance).PayableChargesAccrued;
        int costPerCharge = chargingMainMenu.chargerSO.costPerCharge;
        EquipmentInstance equipmentInstance = inventorySlotUI.gearInstance as EquipmentInstance;
        EquipmentSO equipmentSO = equipmentInstance.gearSO as EquipmentSO;

        feeTMP.text = "Charging Fee: " + (chargesAccrued * costPerCharge) + " $MAMS";
        chargeAmountTMP.text = "Charge: " + equipmentInstance.Charge + " / " + equipmentSO.maxPotential;
    }

    public void ChargingSlotSelected(InventorySlotUI chargingSlotSelected)
    {
        if (isEquipping)
        {
            EquipmentInstance gearInstanceToCharge = chargingMenuManager.chargingEquipmentSelectMenu.equipmentInstanceToCharge;

            if (chargingSlotSelected.gearInstance.gearSO != null)
            {
                if (TryRetrieveGear(chargingSlotSelected))
                    headerTMP.text = "Charge " + gearInstanceToCharge.gearSO.gearName + " in slot " + (slotSelectedIndex + 1) + "?";
                return;
            }

            var chargingSlots = chargingMenuManager.chargingMainMenu.chargerSO.chargingSlots;

            chargingSlots[slotSelectedIndex] = gearInstanceToCharge;
            chargingMainMenu.playerInventorySO.RemoveGearFromInventory(gearInstanceToCharge, true);

            SyncSlotFromSO(chargingSlotSelected);
            chargingMenuManager.chargingEquipmentSelectMenu.InitialiseInventoryUI();

            UpdateHeader(chargingSlotSelected);

            isEquipping = false;
            chargingSlotUIs[slotSelectedIndex].onHighlighted();
        }

        else
        {
            if (chargingSlotSelected.gearInstance.gearSO == null)
            {
                ExitMenu();
                chargingMenuManager.DisplaySubMenu(chargingMenuManager.chargingEquipmentSelectMenu);
                chargingMenuManager.EnterMenu(chargingMenuManager.chargingEquipmentSelectMenu);
            }

            else
            {
                if (TryRetrieveGear(chargingSlotSelected))
                    chargingSlotSelected.onHighlighted();
            }
        }
    }

    bool TryRetrieveGear(InventorySlotUI chargingSlotSelected)
    {
        int chargesAccrued = ((EquipmentInstance)chargingSlotSelected.gearInstance).PayableChargesAccrued;
        int costPerCharge = chargingMainMenu.chargerSO.costPerCharge;
        int fee = chargesAccrued * costPerCharge;
        var stats = chargingMainMenu.playerPermanentStats;

        if (fee > stats.Smams)
        { 
            smamsInventoryTMP.GetComponent<Animator>().Play("SmamsRedText");
            headerTMP.text = "Unable to retrieve, Insufficient $MAMS";
            return false;
        }

        bool inventorySpaceAvailable = chargingMainMenu.playerInventorySO.AttemptAddGearToInventory(chargingSlotSelected.gearInstance, true);

        if (inventorySpaceAvailable)
        {
            smamsInventoryTMP.GetComponent<Animator>().Play("SmamsRedText");

            int smamsInitialValue = stats.Smams;
            int smamsFinalValue = stats.Smams -= fee;

            stats.Smams = smamsFinalValue;

            StartCoroutine(FieldEvents.LerpValuesCoRo(smamsInitialValue, smamsFinalValue, .5f,
                smamsValue =>
                {
                    smamsInventoryTMP.text = "$MAMS: " + Mathf.RoundToInt(smamsValue).ToString("N0");
                }));

            ((EquipmentInstance)chargingSlotSelected.gearInstance).ResetPayableChargesAccrued();
            chargingMainMenu.chargerSO.chargingSlots[slotSelectedIndex] = new EquipmentInstance();
            SyncSlotFromSO(chargingSlotSelected);
            chargingMenuManager.chargingEquipmentSelectMenu.InitialiseInventoryUI();

            return true;
        }

        else
        {
            headerTMP.text = $"Unable to retrieve, Inventory full";
            return false;
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

        chargeAmountTMP.text = "";
        gearDescriptionTMP.text = "";

        chargingMainMenu.menuButtonHighlighteds[0].button.interactable = true;
        chargingMainMenu.menuButtonHighlighteds[0].SetButtonNormalColor(Color.white);
        chargingMenuManager.chargingMainMenu.firstButtonToSelect = chargingMenuManager.chargingMainMenu.menuButtonHighlighteds[0].button;

        chargingMenuManager.EnterMenu(chargingMenuManager.chargingMainMenu);
    }

    public void CheckCharges()
    {
        chargeTimer += Time.deltaTime;

        float magicNumber = 1.5f;
        float timerDuration = magicNumber / chargingMainMenu.chargerSO.chargingSlots.Length;

        if (chargeTimer < timerDuration)
            return;

        chargeTimer -= timerDuration;

        TickCharge();
    }

    void TickCharge()
    {
        var slots = chargingMainMenu.chargerSO.chargingSlots;

        var slot = slots[rotatingChargerIndex];

        if (slot.gearSO == null || slot.ChargePercentage() >= 100)
        {
            rotatingChargerIndex = (rotatingChargerIndex + 1) % chargingSlotUIs.Count;
            return;
        }

        slot.AddCharge(1f);
        slot.AddAccruedCharge(1);

        var slotUI = chargingSlotUIs[rotatingChargerIndex];

        slotUI.itemQuantityTMP.text = ": " + slot.ChargePercentage() + "%";
        charingSlotTMPanimators[rotatingChargerIndex].Play("ChargingSlotTMPAnimation");
        rotatingChargerIndex = (rotatingChargerIndex + 1) % chargingSlotUIs.Count;

        if (chargingMenuManager.menuUpdateMethod == this)
            UpdateEquipmentChargeDisplay(chargingSlotUIs[slotSelectedIndex]);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            slotSelectedIndex = 0; //don't merge with ExitMenu func as it's used elsewhere
            isEquipping = false;
            ExitMenu();
        }

        CheckCharges();
    }
}
