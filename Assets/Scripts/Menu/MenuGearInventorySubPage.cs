using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class MenuGearInventorySubPage : PauseMenu
{
    public Button firstButtonToSelect;
    public MenuGearMainPage menuGearMainPage;
    public MenuGearEquipSubPage menuGearEquipSubPage;
    public List<InventorySlotUI> inventorySlots = new List<InventorySlotUI>();
    public TextMeshProUGUI gearDescriptionTMP;
    public TextMeshProUGUI equipmentCharge;
    public TextMeshProUGUI gearValueTMP;
    public TextMeshProUGUI gearEquipStatusTMP;
    public GameObject inventorySlotUIPrefab, noneGO;
    public GameObject inventorySlotsParent;
    public int highlightedButtonIndex;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public void InitialiseInventoryUI()
    {
        var inventorySO = menuGearMainPage.playerInventorySO;

            DeleteAllInventoryUI();

        for (int i = 0; i < inventorySO.gearInstanceInventory.Count; i++)
        {
            GameObject UIgearSlotGO = Instantiate(inventorySlotUIPrefab);
            UIgearSlotGO.transform.SetParent(inventorySlotsParent.transform, false);

            UIgearSlotGO.name = "gear slot " + i;
            InventorySlotUI inventorySlotUI = UIgearSlotGO.GetComponent<InventorySlotUI>();

            inventorySlotUI.itemNameTMP.text = "";
            inventorySlotUI.itemQuantityTMP.text = "";
            inventorySlotUI.icon.sprite = inventorySlotUI.freeIcon;

            if (i < inventorySO.gearInstanceInventory.Count && inventorySO.gearInstanceInventory[i] != null)
            {
                var gearInstance = inventorySO.gearInstanceInventory[i];

                inventorySlotUI.gearInstance = gearInstance;
                inventorySlotUI.itemNameTMP.text = gearInstance.gearSO.gearName;
                inventorySlotUI.itemQuantityTMP.text = inventorySlotUI.gearInstance.GearQuantityRemainingString();

                bool isEquipment = gearInstance.gearSO is EquipmentSO;
                inventorySlotUI.icon.sprite = isEquipment ? inventorySlotUI.equipmentIcon : inventorySlotUI.consumableIcon;

                bool isCurrentlyEquipped = gearInstance.isCurrentlyEquipped;
                FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.white, isCurrentlyEquipped ? .7f : 1);
                FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.white, isCurrentlyEquipped ? .7f : 1);

                if (!isCurrentlyEquipped)
                    inventorySlotUI.button.onClick.AddListener(() => OnInventorySlotSelected(inventorySlotUI));

                inventorySlotUI.onHighlighted = () =>
                {
                    OnInventorySlotHighlighted(inventorySlotUI);
                };

                inventorySlotUI.onUnHighlighted = () =>
                {
                    FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.white, inventorySlotUI.itemNameTMP.alpha);
                    FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.white, inventorySlotUI.itemNameTMP.alpha);
                };

                inventorySlots.Add(inventorySlotUI);
            }
        }

        List<Button> inventorySlotButtons = new();

        foreach (var inventorySlot in inventorySlots)
            if (inventorySlot.gearInstance != null) inventorySlotButtons.Add(inventorySlot.button);

        FieldEvents.SetGridNavigationWrapAroundHorizontal(inventorySlotButtons, 3);
    }

    public void SetInventorySlotColor(InventorySlotUI inventorySlot, Color normalColor)
    {
        float alpha = inventorySlot.gearInstance.isCurrentlyEquipped ? 0.6f : 1f;
        FieldEvents.SetTextColor(inventorySlot.itemNameTMP, normalColor, alpha);
        FieldEvents.SetTextColor(inventorySlot.itemQuantityTMP, normalColor,  alpha);
    }

    string ItemQuantityRemaining(GearInstance gearInstance)
    {
        if (gearInstance is EquipmentInstance equipmentInstance)
            return ": " + equipmentInstance.ChargePercentage() + "%";
        if (gearInstance is ConsumableInstance consumableInstance)
            return "x " + consumableInstance.quantityAvailable;
        return "";
    }

    public void DeleteAllInventoryUI()
    {
        inventorySlots.Clear();

        for (int i = inventorySlotsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventorySlotsParent.transform.GetChild(i).gameObject);
        }
    }

    public override void EnterMenu()
    {
        var inventory = menuGearMainPage.playerInventorySO.gearInstanceInventory;
        if (inventory.TrueForAll(x => x == null))
            return;

        DisplayMenu(true);
        firstButtonToSelect.Select();
    }

    public override void ExitMenu()
    {
        pauseMenuManager.EnterMenu(pauseMenuManager.gearPageSelection);
        menuGearMainPage.inventoryHighlightedButton.button.Select();
        menuGearMainPage.inventoryHighlightedButton.SetButtonNormalColor(Color.white);
    }

    public void OnInventorySlotSelected(InventorySlotUI inventorySlot)
    {
        if (inventorySlot.gearInstance.isCurrentlyEquipped) return;

        highlightedButtonIndex = inventorySlots.IndexOf(inventorySlot);

        firstButtonToSelect = inventorySlot.button;

        menuGearEquipSubPage.isEquipping = true;
        menuGearEquipSubPage.pageHeaderTMP.text = "Equip " + inventorySlot.gearInstance.gearSO.gearName + "?";
        menuGearEquipSubPage.equipPageHeaderGO.SetActive(true);
        displayContainer.SetActive(false);
        menuGearMainPage.displayContainer.SetActive(false);
        pauseMenuManager.EnterMenu(pauseMenuManager.gearEquipSubPage);
    }

    public void OnInventorySlotHighlighted(InventorySlotUI inventorySlot)
    {
        SetInventorySlotColor(inventorySlot, Color.yellow);
        UpdateGearPropertiesTMPs(inventorySlot);
        highlightedButtonIndex = inventorySlots.IndexOf(inventorySlot);
    }

    void UpdateGearPropertiesTMPs(InventorySlotUI inventorySlot)
    {
        var gearInstance = inventorySlot.gearInstance;

        gearValueTMP.text = $"Sell Value: {gearInstance.gearSO.value:N0} $MAMS";
        gearDescriptionTMP.text = gearInstance.gearSO.gearDescription;

        if (gearInstance is EquipmentInstance equipmentInstance) equipmentCharge.text = equipmentInstance.ChargeTotalString();
        else equipmentCharge.text = "";

        // Equip status
        if (gearInstance.isCurrentlyEquipped)
        {
            int slotIndex = menuGearMainPage.playerInventorySO.gearInstanceEquipped.IndexOf(gearInstance) + 1;
            gearEquipStatusTMP.text = $"Equipped to Slot {slotIndex}. CTRL to unequip";
        }

        else
            gearEquipStatusTMP.text = "SELECT to equip";
    }

    public void UnequipHighlightedGearInstance(GearInstance gearInstance)
    {
        int i = menuGearMainPage.playerInventorySO.gearInstanceEquipped.IndexOf(gearInstance);

        menuGearMainPage.playerInventorySO.UnequipGearFromSlot(menuGearMainPage.playerInventorySO.gearInstanceEquipped[i]);
        InitialiseInventoryUI();
        menuGearEquipSubPage.InitialiseEquipSlots();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            inventorySlots[highlightedButtonIndex].onUnHighlighted();
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
