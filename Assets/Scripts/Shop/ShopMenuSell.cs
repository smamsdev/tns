using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopSellMenu : ShopMenu
{
    public Button firstButtonToSelect;
    public GameObject UIInventorySlotPrefab, inventorySlotsParent, noneGO;
    public List<Button> inventorySlotButtons = new List<Button>();
    int highlightedButtonIndex;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public void InstantiateUIInventorySlots()
    {
        DeleteAllInventoryUI();

        var gearInventory = shopMenuManagerUI.playerInventory.inventorySO.gearInventory;

        if (gearInventory == null || gearInventory.Count == 0)
        {
            noneGO.SetActive(true);
            return;
        }

        noneGO.SetActive(false);

        foreach (GearSO gear in gearInventory)
        {
            GameObject UIInventorySlotGO = Instantiate(UIInventorySlotPrefab);
            UIInventorySlotGO.transform.SetParent(inventorySlotsParent.transform, false);

            InventorySlotUI inventorySlot = UIInventorySlotGO.GetComponent<InventorySlotUI>();
            inventorySlot.gear = gear;
            inventorySlot.itemNameTMP.text = gear.gearName;
            inventorySlot.itemQuantityTMP.text = ItemQuantityRemaining(gear);

            bool isEquipment = gear is EquipmentSO;
            SetInventorySlotColor(inventorySlot, isEquipment ? inventorySlot.equipmentColor : inventorySlot.consumableColor);

            inventorySlotButtons.Add(inventorySlot.button);

            inventorySlot.button.onClick.AddListener(() => SellGearInSlot(gear));

            inventorySlot.onHighlighted = () =>
            {
                shopMenuManagerUI.UpdateDescriptionField(gear);
                SetInventorySlotColor(inventorySlot, Color.yellow);
                highlightedButtonIndex = inventorySlotButtons.IndexOf(inventorySlot.button);
            };

            inventorySlot.onUnHighlighted = () =>
            {
                SetInventorySlotColor(inventorySlot, isEquipment ? inventorySlot.equipmentColor : inventorySlot.consumableColor);
            };

            UIInventorySlotGO.name = gear.gearName;
        }

        FieldEvents.SetGridNavigationWrapAround(inventorySlotButtons, 5);
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
        inventorySlotButtons.Clear();

        for (int i = inventorySlotsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventorySlotsParent.transform.GetChild(i).gameObject);
        }
    }

    public override void EnterMenu()
    {
        if (shopMenuManagerUI.playerInventory.inventorySO.gearInventory.Count > 0)
        {
            shopMenuManagerUI.mainShopMenuButtons[1].ButtonSelectedAndDisabled();
            shopMenuManagerUI.GearDescriptionGO.SetActive(true);
            shopMenuManagerUI.mainMenu.firstMenuButton = shopMenuManagerUI.mainShopMenuButtons[1].button;
            if (!firstButtonToSelect) { firstButtonToSelect = inventorySlotButtons[0]; }
            firstButtonToSelect.Select();
        }
    }

    public override void ExitMenu()
    {
        shopMenuManagerUI.WireAllMainButtons();
        shopMenuManagerUI.menuUpdateMethod = shopMenuManagerUI.mainMenu;
        shopMenuManagerUI.EnterSubMenu(shopMenuManagerUI.mainMenu);
        shopMenuManagerUI.mainShopMenuButtons[1].SetButtonNormalColor(Color.white);
    }

    public void SellGearInSlot(GearSO gearToSell)
    { 
        shopMenuManagerUI.playerInventory.RemoveGearFromInventory(gearToSell);
        InstantiateUIInventorySlots();

        if (inventorySlotButtons.Count == 0)
        {
            ExitMenu();
            return;
        }

        if (highlightedButtonIndex >= inventorySlotButtons.Count)
        {
            highlightedButtonIndex = inventorySlotButtons.Count - 1;
        }

        highlightedButtonIndex = Mathf.Clamp(
            highlightedButtonIndex,
            0,
            inventorySlotButtons.Count - 1
        );

        firstButtonToSelect = inventorySlotButtons[highlightedButtonIndex];
        firstButtonToSelect.Select();

        var stats = shopMenuManagerUI.player.GetComponent<PlayerCombat>().playerPermanentStats;

        stats.Smams += gearToSell.value;
        shopMenuManagerUI.mainMenu.smamsValue.text = stats.Smams.ToString();
        shopMenuManagerUI.smamsColorAnimator.SetTrigger("plus");
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
}
