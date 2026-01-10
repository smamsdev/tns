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
    public Dictionary<GearSO, InventorySlotUI> gearToSlot = new Dictionary<GearSO, InventorySlotUI>();

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public void InstantiateUIInventorySlots()
    {
        DeleteAllInventoryUI();

        if (playerInventory.inventorySO.gearInventory == null || playerInventory.inventorySO.gearInventory.Count == 0)
        {
            noneGO.SetActive(true);
            return;
        }

        noneGO.SetActive(false);
   
        foreach (GearSO gear in playerInventory.inventorySO.gearInventory)
        {
            GameObject UIgearSlot = Instantiate(UIFieldMenuGearSlotPrefab);
            UIgearSlot.transform.SetParent(inventorySlotsParent.transform, false);

            InventorySlotUI inventorySlot = UIgearSlot.GetComponent<InventorySlotUI>();
            inventorySlot.gear = gear;
            inventorySlot.menuGearInventorySubPage = this;

            inventorySlot.itemNameTMP.text = gear.gearName;

            inventorySlot.itemQuantityTMP.text = ItemQuantityRemaining(inventorySlot.gear);

            bool isEquipment = gear is EquipmentSO;
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

            UIgearSlot.name = gear.gearName;

            gearSlots.Add(inventorySlot);
            slotButtons.Add(inventorySlot.button);
            gearToSlot[gear] = inventorySlot;
        }

        FieldEvents.SetGridNavigationWrapAround(slotButtons, 5);
    }

    void SetInventorySlotColor(InventorySlotUI inventorySlot, Color normalColor)
    {
        inventorySlot.itemNameTMP.color = normalColor;
        inventorySlot.itemQuantityTMP.color = normalColor;

        float alpha = inventorySlot.gear.isCurrentlyEquipped ? 0.7f : 1f;
        FieldEvents.SetTextAlpha(inventorySlot.itemNameTMP, alpha);
        FieldEvents.SetTextAlpha(inventorySlot.itemQuantityTMP, alpha);
    }

    string ItemQuantityRemaining(GearSO gearSO)
    {
        if (gearSO is EquipmentSO equipment)
            return ": " + equipment.Potential + "%";
        if (gearSO is ConsumbableSO consumable)
            return "x " + consumable.quantityAvailable;
        return "";
    }

    public void DeleteAllInventoryUI()
    {
        gearSlots.Clear();
        slotButtons.Clear();
        gearToSlot.Clear();

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
        if (inventorySlot.gear.isCurrentlyEquipped) return;

        inventorySlotSelected = inventorySlot;

        firstButtonToSelect = inventorySlot.button;

        menuGearEquipSubPage.isEquipping = true;
        menuGearEquipSubPage.pageHeaderTMP.text = "Equip " + inventorySlot.gear.gearName + "?";
        menuGearEquipSubPage.equipPageHeaderGO.SetActive(true);
        displayContainer.SetActive(false);
        menuGearPageSelection.displayContainer.SetActive(false);
        menuManagerUI.EnterMenu(menuManagerUI.gearEquipSubPage);
    }

    public void GearSlotHighlighted(InventorySlotUI inventorySlot)
    {
        gearValueTMP.text = "Value: " + inventorySlot.gear.value.ToString() + " $MAMS";
        inventorySlotHighlighted = inventorySlot;

        if (inventorySlot.gear is EquipmentSO)
        {
            gearTypeTMP.text = "Type: Equipment";
        }

        else if (inventorySlot.gear is ConsumbableSO)
        {
            gearTypeTMP.text = "Type: Consumable";
        }

        if (inventorySlot.gear.isCurrentlyEquipped)
        {
            gearDescriptionTMP.text = inventorySlot.gear.gearDescription;
            gearEquipStatusTMP.text = "Equipped to Slot " + (playerInventory.inventorySO.equippedGear.IndexOf(inventorySlot.gear) + 1) + ". PRESS CTRL TO REMOVE";
        }
        else
        {
            gearDescriptionTMP.text = inventorySlot.gear.gearDescription;
            gearEquipStatusTMP.text = "SELECT to equip";
        }
    }

    public void UnequipHighlightedGear(GearSO gear)
    {
        playerInventory.UnequipGearFromSlot(gear);

        var inventorySlot = gearToSlot[gear];
        Debug.Log(gearToSlot[gear]);
        gearEquipStatusTMP.text = "Unequipped";
        FieldEvents.SetTextAlpha(inventorySlot.itemNameTMP, 1f);
        FieldEvents.SetTextAlpha(inventorySlot.itemQuantityTMP, 1f);
        inventorySlot.itemQuantityTMP.text = ItemQuantityRemaining(inventorySlot.gear);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (inventorySlotHighlighted.gear.isCurrentlyEquipped)
            {
                DisplayMenu(true);
                GearSlotHighlighted(inventorySlotHighlighted);
                UnequipHighlightedGear(inventorySlotHighlighted.gear);
                //menuGearEquipSubPage.InitialiseEquipSlots();
            }
        }
    }
}
