using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class MenuGearInventorySubPage : Menu
{
    public Button firstButtonToSelect;
    public PlayerInventory playerInventory;
    public MenuGearPageSelection menuGearPageSelection;
    public MenuGearEquipSubPage menuGearEquipSubPage;
    public List<InventorySlotUI> inventorySlots = new List<InventorySlotUI>();

    public TextMeshProUGUI gearDescriptionTMP;
    public TextMeshProUGUI gearTypeTMP;
    public TextMeshProUGUI gearValueTMP;
    public TextMeshProUGUI gearEquipStatusTMP;
    public GameObject inventorySlotUIPrefab, noneGO;
    public GameObject inventorySlotsParent;
    public int highlightedButtonIndex;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public void InstantiateUIInventorySlots()
    {
        DeleteAllInventoryUI();

        if (playerInventory.inventorySO.gearInstanceInventory == null || playerInventory.inventorySO.gearInstanceInventory.Count == 0)
        {
            noneGO.SetActive(true);
            return;
        }

        noneGO.SetActive(false);
   
        foreach (GearInstance gearInstance in playerInventory.inventorySO.gearInstanceInventory)
        {
            GameObject UIgearSlot = Instantiate(inventorySlotUIPrefab);
            UIgearSlot.transform.SetParent(inventorySlotsParent.transform, false);

            InventorySlotUI inventorySlot = UIgearSlot.GetComponent<InventorySlotUI>();
            inventorySlot.gearInstance = gearInstance;
            //inventorySlot.menuGearInventorySubPage = this;

            inventorySlot.itemNameTMP.text = gearInstance.gearSO.gearName;

            inventorySlot.itemQuantityTMP.text = ItemQuantityRemaining(inventorySlot.gearInstance);

            bool isEquipment = gearInstance.gearSO is EquipmentSO;
            SetInventorySlotColor(inventorySlot, isEquipment ? inventorySlot.equipmentColor : inventorySlot.consumableColor);
            inventorySlot.icon.sprite = isEquipment ? inventorySlot.equipmentIcon : inventorySlot.consumableIcon;

            inventorySlot.button.onClick.AddListener(() => OnInventorySlotSelected(inventorySlot));

            inventorySlot.onHighlighted = () => 
            {
                GearSlotHighlighted(inventorySlot);
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

    public void SetInventorySlotColor(InventorySlotUI inventorySlot, Color normalColor)
    {
        inventorySlot.itemNameTMP.color = normalColor;
        inventorySlot.itemQuantityTMP.color = normalColor;

        float alpha = inventorySlot.gearInstance.isCurrentlyEquipped ? 0.6f : 1f;
        FieldEvents.SetTextAlpha(inventorySlot.itemNameTMP, alpha);
        FieldEvents.SetTextAlpha(inventorySlot.itemQuantityTMP, alpha);
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
        if (firstButtonToSelect == null) { firstButtonToSelect = inventorySlots[0].button; }

        DisplayMenu(true);
        firstButtonToSelect.Select();
    }

    public override void ExitMenu()
    {
        menuManagerUI.EnterMenu(menuManagerUI.gearPageSelection);
        menuGearPageSelection.inventoryHighlightedButton.button.Select();
        menuGearPageSelection.inventoryHighlightedButton.SetButtonNormalColor(Color.white);
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
        menuGearPageSelection.displayContainer.SetActive(false);
        menuManagerUI.EnterMenu(menuManagerUI.gearEquipSubPage);
    }

    public void GearSlotHighlighted(InventorySlotUI inventorySlot)
    {
        var gearInstance = inventorySlot.gearInstance;
        var gearSO = gearInstance.gearSO;

        highlightedButtonIndex = inventorySlots.IndexOf(inventorySlot);

        gearValueTMP.text =
            $"Sell Value: {gearSO.value:N0} $MAMS";

        gearDescriptionTMP.text = gearSO.gearDescription;

        // Gear type
        if (gearSO is EquipmentSO)
            gearTypeTMP.text = "Type: Equipment";
        else if (gearSO is ConsumbableSO)
            gearTypeTMP.text = "Type: Consumable";

        // Equip status
        if (gearInstance.isCurrentlyEquipped)
        {
            int slotIndex = playerInventory.inventorySO.gearInstanceEquipped.IndexOf(gearInstance) + 1;

            gearEquipStatusTMP.text = $"Equipped to Slot {slotIndex}. CTRL to unequip";
        }
        else
        {
            gearEquipStatusTMP.text = "SELECT to equip";
        }
    }

    public void UnequipHighlightedGearInstance(GearInstance gearInstance)
    {
        int i = playerInventory.inventorySO.gearInstanceEquipped.IndexOf(gearInstance);

        playerInventory.UnequipGearFromSlot(playerInventory.inventorySO.gearInstanceEquipped[i]);
        InstantiateUIInventorySlots();
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
