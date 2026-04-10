using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopSellMenu : ShopMenu
{
    public Button firstButtonToSelect;
    public GameObject inventorySlotUIPrefab, inventorySlotsParent, noneGO;
    public List<InventorySlotUI> inventorySlots = new List<InventorySlotUI>();
    public int highlightedButtonIndex;

    public Animator smamsColorAnimator;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public void InitialiseInventoryUI()
    {
        var inventorySO = shopMenuManager.mainMenu.playerInventory.inventorySO;

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
                inventorySlotUI.itemQuantityTMP.text = FieldEvents.ItemQuantityRemaining(inventorySlotUI.gearInstance);

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

    void OnInventorySlotHighlighted(InventorySlotUI inventorySlotUI)
    {
        shopMenuManager.mainMenu.UpdateDescriptionField(inventorySlotUI.gearInstance.gearSO);
        FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.yellow, inventorySlotUI.itemNameTMP.alpha);
        FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.yellow, inventorySlotUI.itemNameTMP.alpha);
        highlightedButtonIndex = inventorySlots.IndexOf(inventorySlotUI);
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
        var inventory = shopMenuManager.mainMenu.playerInventory.inventorySO.gearInstanceInventory;
        if (inventory.TrueForAll(x => x == null))
            return;

        shopMenuManager.mainMenu.DisplayMainButtons(false);
        shopMenuManager.mainMenu.SetHeaderTMP("Select GEAR to sell");

        shopMenuManager.mainMenu.GearDescriptionGO.SetActive(true);
        shopMenuManager.mainMenu.firstMenuButton = shopMenuManager.mainMenu.mainShopMenuButtons[1].button;
        if (!firstButtonToSelect)
            firstButtonToSelect = inventorySlots[0].button;

        firstButtonToSelect.Select();
    }

    public override void ExitMenu()
    {

        shopMenuManager.mainMenu.DisplayMainButtons(true);
        shopMenuManager.mainMenu.SetHeaderTMP("");

        shopMenuManager.menuUpdateMethod = shopMenuManager.mainMenu;
        shopMenuManager.EnterMenu(shopMenuManager.mainMenu);
        shopMenuManager.mainMenu.mainShopMenuButtons[1].SetButtonNormalColor(Color.white);
    }

    public void OnInventorySlotSelected(InventorySlotUI inventorySlotUI)
    {
        if (inventorySlotUI.gearInstance.isCurrentlyEquipped)
            return;

        shopMenuManager.mainMenu.playerInventory.inventorySO.RemoveGearFromInventory(inventorySlotUI.gearInstance, true);
        InitialiseInventoryUI();

        var inventorySO = shopMenuManager.mainMenu.playerInventory.inventorySO;
        if (inventorySO.gearInstanceInventory.TrueForAll(x => x == null))
        {
            ExitMenu();
            return;
        }

        highlightedButtonIndex = Mathf.Clamp(highlightedButtonIndex, 0, inventorySlots.Count - 1);

        firstButtonToSelect = inventorySlots[highlightedButtonIndex].button;
        firstButtonToSelect.Select();

        var stats = shopMenuManager.mainMenu.playerPermanentStats;

        stats.Smams += inventorySlotUI.gearInstance.gearSO.value;
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
