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
    public int highlightedButtonIndex;

    public Animator smamsColorAnimator;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public void InstantiateUIInventorySlots()
    {
        DeleteAllInventoryUI();

        var gearInstanceInventory = shopMenuManager.mainMenu.playerInventory.inventorySO.gearInstanceInventory;

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
            inventorySlot.itemQuantityTMP.text = ItemQuantityRemaining(inventorySlot.gearInstance);

            bool isEquipment = gearInstance.gearSO is EquipmentSO;
            SetInventorySlotColor(inventorySlot, isEquipment ? inventorySlot.equipmentColor : inventorySlot.consumableColor);

            inventorySlotButtons.Add(inventorySlot.button);

            inventorySlot.button.onClick.AddListener(() => SellGearInSlot(gearInstance));

            inventorySlot.onHighlighted = () =>
            {
                shopMenuManager.mainMenu.UpdateDescriptionField(gearInstance.gearSO);
                SetInventorySlotColor(inventorySlot, Color.yellow);
                highlightedButtonIndex = inventorySlotButtons.IndexOf(inventorySlot.button);
            };

            inventorySlot.onUnHighlighted = () =>
            {
                SetInventorySlotColor(inventorySlot, isEquipment ? inventorySlot.equipmentColor : inventorySlot.consumableColor);
            };

            UIInventorySlotGO.name = gearInstance.gearSO.gearName;
        }

        FieldEvents.SetGridNavigationWrapAroundHorizontal(inventorySlotButtons, 3);
    }

    void SetInventorySlotColor(InventorySlotUI inventorySlot, Color normalColor)
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
            return ": " + equipmentInstance.ChargePercentage() + "%";
        if (gearInstance is ConsumableInstance consumableInstance)
            return "x " + consumableInstance.quantityAvailable;
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
        if (shopMenuManager.mainMenu.playerInventory.inventorySO.gearInstanceInventory.Count > 0)
        {
            shopMenuManager.mainMenu.mainShopMenuButtons[1].ButtonSelectedAndDisabled();
            shopMenuManager.mainMenu.GearDescriptionGO.SetActive(true);
            shopMenuManager.mainMenu.firstMenuButton = shopMenuManager.mainMenu.mainShopMenuButtons[1].button;
            if (!firstButtonToSelect) { firstButtonToSelect = inventorySlotButtons[0]; }
            firstButtonToSelect.Select();
        }
    }

    public override void ExitMenu()
    {
        shopMenuManager.menuUpdateMethod = shopMenuManager.mainMenu;
        shopMenuManager.EnterMenu(shopMenuManager.mainMenu);
        shopMenuManager.mainMenu.mainShopMenuButtons[1].SetButtonNormalColor(Color.white);
    }

    public void SellGearInSlot(GearInstance gearInstanceToSell)
    {
        if (gearInstanceToSell.isCurrentlyEquipped)
            return;

        shopMenuManager.mainMenu.playerInventory.RemoveGearFromInventory(gearInstanceToSell);
        InstantiateUIInventorySlots();

        if (inventorySlotButtons.Count == 0)
        {
            ExitMenu();
            return;
        }

        highlightedButtonIndex = Mathf.Clamp(highlightedButtonIndex, 0, inventorySlotButtons.Count - 1);

        firstButtonToSelect = inventorySlotButtons[highlightedButtonIndex];
        firstButtonToSelect.Select();

        var stats = shopMenuManager.mainMenu.playerPermanentStats;

        stats.Smams += gearInstanceToSell.gearSO.value;
        shopMenuManager.mainMenu.smamsInventoryTMP.text = stats.Smams.ToString("N0");
        shopMenuManager.mainMenu.smamsColorAnimator.SetTrigger("plus");
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
}
