using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargingEquipmentSelectMenu : ChargingMenu
{
    public GameObject inventorySlotUIPrefab, noneGO, inventorySlotsParent, propertiesGO;
    public ChargingMainMenu chargingMainMenu;
    public ChargingSlotMenu chargingSlotMenu;
    public TextMeshProUGUI gearDescriptionTMP, gearTypeTMP, gearEquipStatusTMP;

    [Header("Dynamic")]

    public List<InventorySlotUI> inventorySlots = new List<InventorySlotUI>();
    public int highlightedButtonIndex;
    public EquipmentInstance equipmentInstanceToCharge;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public override void EnterMenu()
    {
        if (inventorySlots.Count <= 0) 
        {
            ExitMenu();
            return;
        }
            
        chargingMainMenu.menuButtonHighlighteds[1].SetButtonNormalColor(Color.yellow);
        chargingMainMenu.menuButtonHighlighteds[1].button.interactable = false;

        propertiesGO.SetActive(true);

        inventorySlots[0].button.Select();
    }

    public void InitialiseInventoryUI()
    {
        DeleteAllInventoryUI();
        ClearText();

        var gearInstanceInventory = chargingMainMenu.playerInventory.inventorySO.gearInstanceInventory;

        if (gearInstanceInventory == null || gearInstanceInventory.Count == 0)
        {
            noneGO.SetActive(true);
            return;
        }

        noneGO.SetActive(false);

        foreach (GearInstance gearInstance in gearInstanceInventory)
        {
            GameObject UIgearSlot = Instantiate(inventorySlotUIPrefab);
            UIgearSlot.transform.SetParent(inventorySlotsParent.transform, false);

            InventorySlotUI inventorySlot = UIgearSlot.GetComponent<InventorySlotUI>();
            inventorySlot.gearInstance = gearInstance;

            inventorySlot.itemNameTMP.text = gearInstance.gearSO.gearName;

            inventorySlot.itemQuantityTMP.text = ItemQuantityRemaining(inventorySlot.gearInstance);

            bool isEquipment = gearInstance.gearSO is EquipmentSO;
            SetInventorySlotColor(inventorySlot, isEquipment ? inventorySlot.equipmentColor : inventorySlot.consumableColor);
            inventorySlot.icon.sprite = isEquipment ? inventorySlot.equipmentIcon : inventorySlot.consumableIcon;
            
            inventorySlot.button.onClick.AddListener(() => OnInventorySlotSelected(inventorySlot));

            inventorySlot.onHighlighted = () =>
            {
                OnInventorySlotHighlighted(inventorySlot);
                SetInventorySlotColor(inventorySlot, Color.yellow);
            };

            inventorySlot.onUnHighlighted = () =>
            {
                SetInventorySlotColor(inventorySlot, isEquipment ? inventorySlot.equipmentColor : inventorySlot.consumableColor);
            };

            UIgearSlot.name = gearInstance.gearSO.gearName;

            inventorySlots.Add(inventorySlot);
        }

        List<Button> inventorySlotButtons = new List<Button>();
        foreach (var inventorySlot in inventorySlots)
            inventorySlotButtons.Add(inventorySlot.button);

        FieldEvents.SetGridNavigationWrapAroundHorizontal(inventorySlotButtons, 3);
    }

    string ItemQuantityRemaining(GearInstance gearInstance)
    {
        if (gearInstance is EquipmentInstance equipmentInstance)
            return ": " + equipmentInstance.ChargePercentage() + "%";

        if (gearInstance is ConsumableInstance consumableInstance)
            return "x " + consumableInstance.quantityAvailable;

        return "";
    }

    public void SetInventorySlotColor(InventorySlotUI inventorySlot, Color normalColor)
    {
        inventorySlot.itemNameTMP.color = normalColor;
        inventorySlot.itemQuantityTMP.color = normalColor;

        float alpha = 0.7f;

        if (inventorySlot.gearInstance is EquipmentInstance && ((EquipmentInstance)inventorySlot.gearInstance).ChargePercentage() != 100)
            alpha = 1f;

        FieldEvents.SetTextAlpha(inventorySlot.itemNameTMP, alpha);
        FieldEvents.SetTextAlpha(inventorySlot.itemQuantityTMP, alpha);
    }

    public void DeleteAllInventoryUI()
    {
        inventorySlots.Clear();

        for (int i = inventorySlotsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventorySlotsParent.transform.GetChild(i).gameObject);
        }
    }

    void ClearText()
    {
        gearDescriptionTMP.text = "";
        gearTypeTMP.text = "";
        gearEquipStatusTMP.text = "";
    }

    public void OnInventorySlotSelected(InventorySlotUI inventorySlot)
    {
        if (inventorySlot.gearInstance.isCurrentlyEquipped || inventorySlot.gearInstance is ConsumableInstance || ((EquipmentInstance)inventorySlot.gearInstance).ChargePercentage() == 100) 
            return;

        highlightedButtonIndex = inventorySlots.IndexOf(inventorySlot);

        equipmentInstanceToCharge = inventorySlot.gearInstance as EquipmentInstance;

        chargingSlotMenu.isEquipping = true;
        ExitMenu();

        chargingSlotMenu.pageHeaderTMP.text = "Start charging " + inventorySlot.gearInstance.gearSO.gearName + "?";
        chargingMenuManager.EnterMenu(chargingMenuManager.ChargingSlotSelectMenu);

        chargingMenuManager.DisplaySubMenu(chargingMenuManager.ChargingSlotSelectMenu);
        chargingMenuManager.ChargingSlotSelectMenu.chargingSlotUIs[0].button.Select();
        chargingMenuManager.ChargingSlotSelectMenu.chargingSlotUIs[0].onHighlighted();
    }

    public void OnInventorySlotHighlighted(InventorySlotUI inventorySlot)
    {
        highlightedButtonIndex = inventorySlots.IndexOf(inventorySlot);

        gearDescriptionTMP.text = "Description: " + inventorySlot.gearInstance.gearSO.gearDescription;

        //Gear Type
        if (inventorySlot.gearInstance is EquipmentInstance equipmentInstance)
        { 
            gearTypeTMP.text = "Charge " + equipmentInstance.Charge + " / " + ((EquipmentSO)equipmentInstance.gearSO).maxPotential;
        }

        else
            gearTypeTMP.text = "Consumable";

        //Availability

        if (inventorySlot.gearInstance is ConsumableInstance)
            gearEquipStatusTMP.text = "";

        else
        {
            if (inventorySlot.gearInstance.isCurrentlyEquipped)
            {
                gearEquipStatusTMP.text = "Equipped to Slot " + (chargingMainMenu.playerInventory.inventorySO.gearInstanceEquipped.IndexOf(inventorySlot.gearInstance) + 1) + ". PRESS CTRL TO REMOVE";
            }

            if (((EquipmentInstance)inventorySlot.gearInstance).ChargePercentage() == 100)
            {
                gearEquipStatusTMP.text = "Fully charged";
            }

            else
            {
                gearEquipStatusTMP.text = "SELECT to Charge";
            }
        }
    }

    public void UnequipHighlightedGearInstance(GearInstance gearInstance)
    {
        int i = chargingMainMenu.playerInventory.inventorySO.gearInstanceEquipped.IndexOf(gearInstance);

        chargingMainMenu.playerInventory.UnequipGearFromSlot(chargingMainMenu.playerInventory.inventorySO.gearInstanceEquipped[i]);
        InitialiseInventoryUI();
    }

    public override void ExitMenu()
    {
        ClearText();

        chargingMenuManager.ChargingSlotSelectMenu.pageHeaderTMP.text = "Select Charging Slot:";

        if (inventorySlots.Count > 0)
            inventorySlots[highlightedButtonIndex].onUnHighlighted();

        chargingMainMenu.menuButtonHighlighteds[1].button.interactable = true;
        chargingMainMenu.menuButtonHighlighteds[1].SetButtonNormalColor(Color.white);
        chargingMenuManager.chargingMainMenu.firstButtonToSelect = chargingMenuManager.chargingMainMenu.menuButtonHighlighteds[1].button;

        chargingMenuManager.EnterMenu(chargingMenuManager.chargingMainMenu);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (inventorySlots[highlightedButtonIndex].gearInstance.isCurrentlyEquipped)
            {
                DisplayMenu(true);
                UnequipHighlightedGearInstance(inventorySlots[highlightedButtonIndex].gearInstance);
                inventorySlots[highlightedButtonIndex].button.Select();
            }
        }
    }
}
