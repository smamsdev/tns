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

    public GameObject propertiesDisplay;
    public float chargeTimer;
    public int rotatingChargerIndex = 0;

    public TextMeshProUGUI feeTMP, gearDescriptionTMP, gearTypeTMP, smamsInventoryTMP, headerTMP;

    public int slotSelectedIndex;
    public bool isEquipping;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
        headerTMP.text = "Charger status";
    }

    public override void EnterMenu()
    {
        chargingMainMenu.menuButtonHighlighteds[0].SetButtonNormalColor(Color.yellow);
        chargingMainMenu.menuButtonHighlighteds[0].button.interactable = false;

        propertiesDisplay.SetActive(true);

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
            InstantiateSlotUIGO(i);
            UpdateSlotDisplay(chargingSlotUIs[i]);
        }

        FieldEvents.SetGridNavigationWrapAround(chargingSlotButtons, chargingSlots.Length);
        UpdateSmamsUI();
    }

    void InstantiateSlotUIGO(int slotIndex)
    {
        var chargerSOGearInstanceSlots = chargingMainMenu.chargerSO.chargingSlots;
        GameObject ChargingSlotGO = Instantiate(chargingSlotPrefab, ChargingSlotUIsParent.transform);
        InventorySlotUI inventorySlotUI = ChargingSlotGO.GetComponent<InventorySlotUI>();

        ChargingSlotGO.name = "Charging Slot " + slotIndex;

        inventorySlotUI.button.onClick.AddListener(() => ChargingSlotSelected(inventorySlotUI));

        inventorySlotUI.onHighlighted = () =>
        {
            ChargingSlotHighlighted(inventorySlotUI);
            UpdateHeader(inventorySlotUI);
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

    void UpdateSlotDisplay(InventorySlotUI inventorySlotUI)
    {
        int index = chargingSlotUIs.IndexOf(inventorySlotUI);
        var slot = chargingMainMenu.chargerSO.chargingSlots[index];

        if (slot == null || slot.gearSO == null)
        {
            inventorySlotUI.gearInstance = null;
            inventorySlotUI.itemNameTMP.text = $"Charging Slot {index + 1}: Empty";
            inventorySlotUI.itemQuantityTMP.text = "";
            return;
        }

        inventorySlotUI.gearInstance = slot;
        inventorySlotUI.itemNameTMP.text = slot.gearSO.name;
        inventorySlotUI.itemQuantityTMP.text = $": {slot.ChargePercentage()}%";
    }

    void ClearText()
    {
        feeTMP.text = "";
        gearDescriptionTMP.text = "";
    }

    void ChargingSlotHighlighted(InventorySlotUI inventorySlotUI)
    {
        slotSelectedIndex = chargingSlotUIs.IndexOf(inventorySlotUI);

        if (inventorySlotUI.gearInstance == null || inventorySlotUI.gearInstance.gearSO == null)
        {
            feeTMP.text = $"Charging Fee: {chargingMainMenu.chargerSO.costPerCharge} $MAMS per charge";
            headerTMP.text = $"Select EQUIPMENT to charge";
            gearTypeTMP.text = "";
        }

        else
        {
            UpdateEquipmentChargeDisplay(inventorySlotUI);
        }
    }

    void UpdateHeader(InventorySlotUI inventorySlotUI)
    {
        if (inventorySlotUI.gearInstance == null || inventorySlotUI.gearInstance.gearSO == null)
        {
            feeTMP.text = $"Charging Fee: {chargingMainMenu.chargerSO.costPerCharge} $MAMS per charge";
            headerTMP.text = $"Select EQUIPMENT to charge";
            gearTypeTMP.text = "";
            return;
        }

        int chargesAccrued = ((EquipmentInstance)inventorySlotUI.gearInstance).ChargesAccrued;
        int costPerCharge = chargingMainMenu.chargerSO.costPerCharge;
        int fee = chargesAccrued * costPerCharge;
        var stats = chargingMainMenu.playerPermanentStats;

        feeTMP.text = "Charging Fee: " + (chargesAccrued * costPerCharge) + " $MAMS";

        gearDescriptionTMP.text = $"Description: {inventorySlotUI.gearInstance.gearSO.gearDescription}";

        EquipmentInstance equipmentInstance = inventorySlotUI.gearInstance as EquipmentInstance;
        EquipmentSO equipmentSO = equipmentInstance.gearSO as EquipmentSO;

        gearTypeTMP.text = "Charge " + equipmentInstance.Charge + " / " + equipmentSO.maxPotential;

        if (fee > stats.Smams)
            headerTMP.text = "Unable to retrieve, Insufficient $MAMS";

        else
            headerTMP.text = $"Retrieve EQUIPMENT from charger";
    }

    void UpdateEquipmentChargeDisplay(InventorySlotUI inventorySlotUI)
    {
        int chargesAccrued = ((EquipmentInstance)inventorySlotUI.gearInstance).ChargesAccrued;
        int costPerCharge = chargingMainMenu.chargerSO.costPerCharge;

        feeTMP.text = "Charging Fee: " + (chargesAccrued * costPerCharge) + " $MAMS";

        gearDescriptionTMP.text = $"Description: {inventorySlotUI.gearInstance.gearSO.gearDescription}";

        EquipmentInstance equipmentInstance = inventorySlotUI.gearInstance as EquipmentInstance;
        EquipmentSO equipmentSO = equipmentInstance.gearSO as EquipmentSO;

        gearTypeTMP.text = "Charge " + equipmentInstance.Charge + " / " + equipmentSO.maxPotential;
    }

    public void ChargingSlotSelected(InventorySlotUI chargingSlotSelected)
    {
        if (isEquipping)
        {
            if (chargingSlotSelected.gearInstance != null && chargingSlotSelected.gearInstance.gearSO != null)
                return;

            EquipmentInstance gearInstanceToCharge = chargingMenuManager.chargingEquipmentSelectMenu.equipmentInstanceToCharge;
            var chargingSlots = chargingMenuManager.chargingMainMenu.chargerSO.chargingSlots;

            chargingSlots[slotSelectedIndex] = gearInstanceToCharge;
            gearInstanceToCharge.StartCharging();
            chargingMainMenu.playerInventory.inventorySO.RemoveGearFromInventory(gearInstanceToCharge, true);

            UpdateSlotDisplay(chargingSlotSelected);
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

        bool inventorySpaceAvailable = chargingMainMenu.playerInventory.inventorySO.AttemptAddGearToInventory(chargingSlotSelected.gearInstance, true);

        if (inventorySpaceAvailable)
        {
            chargingMainMenu.chargerSO.chargingSlots[slotSelectedIndex] = null;
            UpdateSlotDisplay(chargingSlotSelected);
            chargingMenuManager.chargingEquipmentSelectMenu.InitialiseInventoryUI();
            smamsInventoryTMP.GetComponent<Animator>().Play("SmamsRedText");

            StartCoroutine(FieldEvents.LerpValuesCoRo(stats.Smams, stats.Smams -= fee, .2f,
                smamsValue =>
                {
                    smamsInventoryTMP.text = "$MAMS: " + Mathf.RoundToInt(smamsValue).ToString("N0");
                }));
        }

        else
        {
            headerTMP.text = $"Unable to retrieve, Inventory full";
            return;
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
                charingSlotTMPanimators[rotatingChargerIndex].Play("ChargingSlotTMPAnimation");

                if (chargingMenuManager.menuUpdateMethod == this)
                    ChargingSlotHighlighted(chargingSlotUIs[slotSelectedIndex]);
            }
        }

        rotatingChargerIndex = (rotatingChargerIndex + 1) % chargingSlotUIs.Count;
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
