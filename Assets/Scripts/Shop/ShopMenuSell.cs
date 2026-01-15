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

        var gearInstanceInventory = shopMenuManagerUI.playerInventory.inventorySO.gearInstanceInventory;

        if (gearInstanceInventory == null || gearInstanceInventory.Count == 0)
        {
            noneGO.SetActive(true);
            return;
        }

        noneGO.SetActive(false);

        foreach (GearInstance gearInstance in gearInstanceInventory)
        {
            if (gearInstance == null)
            {
                Debug.Log("empty inventory instance, cannot load player inventory");
                return;
            }

            GameObject UIInventorySlotGO = Instantiate(UIInventorySlotPrefab);
            UIInventorySlotGO.transform.SetParent(inventorySlotsParent.transform, false);

            InventorySlotUI inventorySlot = UIInventorySlotGO.GetComponent<InventorySlotUI>();
            inventorySlot.gearInstance = gearInstance;
            inventorySlot.itemNameTMP.text = gearInstance.gearSO.gearName;
            inventorySlot.itemQuantityTMP.text = ItemQuantityRemaining(gearInstance.gearSO);

            bool isEquipment = gearInstance.gearSO is EquipmentSO;
            SetInventorySlotColor(inventorySlot, isEquipment ? inventorySlot.equipmentColor : inventorySlot.consumableColor);

            inventorySlotButtons.Add(inventorySlot.button);

            inventorySlot.button.onClick.AddListener(() => SellGearInSlot(gearInstance));

            inventorySlot.onHighlighted = () =>
            {
                shopMenuManagerUI.UpdateDescriptionField(gearInstance.gearSO);
                SetInventorySlotColor(inventorySlot, Color.yellow);
                highlightedButtonIndex = inventorySlotButtons.IndexOf(inventorySlot.button);
            };

            inventorySlot.onUnHighlighted = () =>
            {
                SetInventorySlotColor(inventorySlot, isEquipment ? inventorySlot.equipmentColor : inventorySlot.consumableColor);
            };

            UIInventorySlotGO.name = gearInstance.gearSO.gearName;
        }

        FieldEvents.SetGridNavigationWrapAround(inventorySlotButtons, 5);
    }

    void SetInventorySlotColor(InventorySlotUI inventorySlot, Color normalColor)
    {
        inventorySlot.itemNameTMP.color = normalColor;
        inventorySlot.itemQuantityTMP.color = normalColor;

        float alpha = inventorySlot.gearInstance.isCurrentlyEquipped ? 0.7f : 1f;
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
        if (shopMenuManagerUI.playerInventory.inventorySO.gearInstanceInventory.Count > 0)
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

    public void SellGearInSlot(GearInstance gearInstanceToSell)
    {
        //shopMenuManagerUI.playerInventory.RemoveGearFromInventory(gearInstanceToSell);
        Debug.Log("wtf");
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

        stats.Smams += gearInstanceToSell.gearSO.value;
        shopMenuManagerUI.mainMenu.smamsInventoryTMP.text = stats.Smams.ToString();
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
