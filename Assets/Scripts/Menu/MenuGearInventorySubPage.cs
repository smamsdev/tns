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
    public GameObject inventorySlotUIPrefab, noneGO, inventorySlotsParent;
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

            if (i < inventorySO.gearInstanceInventory.Count && inventorySO.gearInstanceInventory[i].gearSO != null)
            {
                var gearInstance = inventorySO.gearInstanceInventory[i];

                inventorySlotUI.gearInstance = gearInstance;
                inventorySlotUI.itemNameTMP.text = gearInstance.gearSO.gearName;
                inventorySlotUI.itemQuantityTMP.text = inventorySlotUI.gearInstance.GearQuantityRemainingString();

                bool isEquipment = gearInstance.gearSO is EquipmentSO;
                bool isCurrentlyEquipped = gearInstance.isCurrentlyEquipped;

                inventorySlotUI.icon.sprite = isEquipment ? inventorySlotUI.equipmentIcon : inventorySlotUI.consumableIcon;

                if (!isCurrentlyEquipped)
                    inventorySlotUI.button.onClick.AddListener(() => OnInventorySlotSelected(inventorySlotUI));

                inventorySlotUI.onHighlighted = () =>
                {
                    OnInventorySlotHighlighted(inventorySlotUI);
                };

                inventorySlotUI.onUnHighlighted = () =>
                {
                    menuGearMainPage.SetSlotColor(inventorySlotUI, Color.white);
                };

                inventorySlots.Add(inventorySlotUI);
            }
        }

        List<Button> inventorySlotButtons = new();

        foreach (var inventorySlot in inventorySlots)
            if (inventorySlot.gearInstance != null) inventorySlotButtons.Add(inventorySlot.button);

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
        if (inventory.TrueForAll(x => x.gearSO == null))
            return;

        menuGearMainPage.ShowMainButtons(false);
        DisplayMenu(true);

        foreach (var slot in inventorySlots)
            menuGearMainPage.SetSlotAlpha(slot, !slot.gearInstance.isCurrentlyEquipped);

        if (firstButtonToSelect == null)
            firstButtonToSelect = inventorySlots[0].button;

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

        firstButtonToSelect = inventorySlot.button;
        menuGearEquipSubPage.isEquipping = true;

        foreach (var slot in menuGearEquipSubPage.equipSlots)
            menuGearMainPage.SetSlotAlpha(slot, !slot.gearInstance.isCurrentlyEquipped);

        displayContainer.SetActive(false);
        pauseMenuManager.EnterMenu(pauseMenuManager.gearEquipSubPage);
    }

    public void OnInventorySlotHighlighted(InventorySlotUI inventorySlot)
    {
        var gi = inventorySlot.gearInstance;

        highlightedButtonIndex = inventorySlots.IndexOf(inventorySlot);
        menuGearMainPage.SetSlotColor(inventorySlot, Color.yellow);
        menuGearMainPage.UpdateGearDescriptionTMPs(gi);
        menuGearMainPage.UpdateHeaderTMP(gi.isCurrentlyEquipped? gi.gearSO.gearName + " already equipped" : "Equip " + gi.gearSO.gearName + "?");
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
                UnequipHighlightedGearInstance(inventorySlots[highlightedButtonIndex].gearInstance);
                inventorySlots[highlightedButtonIndex].button.Select();
            }
        }
    }
}
