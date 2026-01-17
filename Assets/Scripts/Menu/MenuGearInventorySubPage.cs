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
    public List<InventorySlotUI> gearSlots = new List<InventorySlotUI>();
    List<Button> slotButtons = new List<Button>();

    public TextMeshProUGUI gearDescriptionTMP;
    public TextMeshProUGUI gearTypeTMP;
    public TextMeshProUGUI gearValueTMP;
    public TextMeshProUGUI gearEquipStatusTMP;
    public InventorySlotUI inventorySlotHighlighted;
    public GameObject UIFieldMenuGearSlotPrefab, noneGO;
    public GameObject inventorySlotsParent;
    public InventorySlotUI inventorySlotSelected;

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
            GameObject UIgearSlot = Instantiate(UIFieldMenuGearSlotPrefab);
            UIgearSlot.transform.SetParent(inventorySlotsParent.transform, false);

            InventorySlotUI inventorySlot = UIgearSlot.GetComponent<InventorySlotUI>();
            inventorySlot.gearInstance = gearInstance;
            inventorySlot.menuGearInventorySubPage = this;

            inventorySlot.itemNameTMP.text = gearInstance.gearSO.gearName;

            inventorySlot.itemQuantityTMP.text = ItemQuantityRemaining(inventorySlot.gearInstance);

            bool isEquipment = gearInstance.gearSO is EquipmentSO;
            SetInventorySlotColor(inventorySlot, isEquipment ? inventorySlot.equipmentColor : inventorySlot.consumableColor);

            inventorySlot.button.onClick.AddListener(() => InventorySlotSelected(inventorySlot));

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

            gearSlots.Add(inventorySlot);
            slotButtons.Add(inventorySlot.button);
        }

        FieldEvents.SetGridNavigationWrapAround(slotButtons, 5);
    }

    public void SetInventorySlotColor(InventorySlotUI inventorySlot, Color normalColor)
    {
        inventorySlot.itemNameTMP.color = normalColor;
        inventorySlot.itemQuantityTMP.color = normalColor;

        float alpha = inventorySlot.gearInstance.isCurrentlyEquipped ? 0.7f : 1f;
        FieldEvents.SetTextAlpha(inventorySlot.itemNameTMP, alpha);
        FieldEvents.SetTextAlpha(inventorySlot.itemQuantityTMP, alpha);
    }

    string ItemQuantityRemaining(GearInstance gearInstance)
    {
        if (gearInstance is EquipmentInstance equipmentInstance)
            return ": " + equipmentInstance.charge + "%";
        if (gearInstance is ConsumableInstance consumableInstance)
            return "x " + consumableInstance.quantityAvailable;
        return "";
    }

    public void DeleteAllInventoryUI()
    {
        gearSlots.Clear();
        slotButtons.Clear();

        for (int i = inventorySlotsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventorySlotsParent.transform.GetChild(i).gameObject);
        }
    }

    public override void EnterMenu()
    {
        if (firstButtonToSelect == null) { firstButtonToSelect = gearSlots[0].button; }

        DisplayMenu(true);
        firstButtonToSelect.Select();
    }

    public override void ExitMenu()
    {
        menuManagerUI.EnterMenu(menuManagerUI.gearPageSelection);
        menuGearPageSelection.inventoryHighlightedButton.button.Select();
        menuGearPageSelection.inventoryHighlightedButton.SetButtonNormalColor(Color.white);
    }

    public void InventorySlotSelected(InventorySlotUI inventorySlot)
    {
        if (inventorySlot.gearInstance.isCurrentlyEquipped) return;

        inventorySlotSelected = inventorySlot;

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
        gearValueTMP.text = "Value: " + inventorySlot.gearInstance.gearSO.value.ToString() + " $MAMS";
        inventorySlotHighlighted = inventorySlot;

        if (inventorySlot.gearInstance.gearSO is EquipmentSO)
        {
            gearTypeTMP.text = "Type: Equipment";
        }

        else if (inventorySlot.gearInstance.gearSO is ConsumbableSO)
        {
            gearTypeTMP.text = "Type: Consumable";
        }

        if (inventorySlot.gearInstance.isCurrentlyEquipped)
        {
            gearDescriptionTMP.text = inventorySlot.gearInstance.gearSO.gearDescription;
            gearEquipStatusTMP.text = "Equipped to Slot " + (playerInventory.inventorySO.gearInstanceEquipped.IndexOf(inventorySlot.gearInstance) + 1) + ". PRESS CTRL TO REMOVE";
        }
        else
        {
            gearDescriptionTMP.text = inventorySlot.gearInstance.gearSO.gearDescription;
            gearEquipStatusTMP.text = "SELECT to equip";
        }
    }

    public void UnequipHighlightedGearInstance(GearInstance gearInstance)
    {
        int i = playerInventory.inventorySO.gearInstanceEquipped.IndexOf(gearInstance);

        playerInventory.UnequipGearFromSlot(playerInventory.inventorySO.gearInstanceEquipped[i]);
        InstantiateUIInventorySlots();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (inventorySlotHighlighted.gearInstance.isCurrentlyEquipped)
            {
                DisplayMenu(true);
                GearSlotHighlighted(inventorySlotHighlighted);
                UnequipHighlightedGearInstance(inventorySlotHighlighted.gearInstance);
                //menuGearEquipSubPage.InitialiseEquipSlots();
            }
        }
    }
}
