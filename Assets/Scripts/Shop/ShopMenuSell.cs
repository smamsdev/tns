using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class ShopSellMenu : ShopMenu
{
    public Button firstButtonToSelect;
    public GameObject inventorySlotUIPrefab, inventorySlotsParent, noneGO;
    public List<InventorySlotUI> inventorySlots = new List<InventorySlotUI>();
    public int highlightedButtonIndex;
    Coroutine smamsCoroutine;
    float currentlyDisplayedSmams;

    public Animator smamsColorAnimator;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public void InitialiseInventoryUI()
    {
        var inventorySO = shopMenuManager.mainMenu.playerInventorySO;
        currentlyDisplayedSmams = shopMenuManager.mainMenu.playerPermanentStats.Smams;

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
                inventorySlotUI.itemNameTMP.text = gearInstance.gearSO.GearName;
                inventorySlotUI.itemQuantityTMP.text = inventorySlotUI.gearInstance.QuantityString();

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
        shopMenuManager.mainMenu.UpdateDescriptionField(inventorySlotUI.gearInstance);
        shopMenuManager.mainMenu.SetHeaderTMP("Sell " + inventorySlotUI.gearInstance.gearSO.GearName + " ?");
        FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.yellow, inventorySlotUI.itemNameTMP.alpha);
        FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.yellow, inventorySlotUI.itemNameTMP.alpha);
        highlightedButtonIndex = inventorySlots.IndexOf(inventorySlotUI);
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
        var gearInstanceInventory = shopMenuManager.mainMenu.playerInventorySO.gearInstanceInventory;

        if (gearInstanceInventory.TrueForAll(x => x.gearSO == null))
        {
            shopMenuManager.menuUpdateMethod = shopMenuManager.mainMenu;
            return;
        }

        shopMenuManager.mainMenu.DisplayMainButtons(false);
        shopMenuManager.mainMenu.firstMenuButton = shopMenuManager.mainMenu.mainShopMenuButtons[1].button;

        if (!firstButtonToSelect)
            firstButtonToSelect = inventorySlots[0].button;

        firstButtonToSelect.Select();
    }

    public override void ExitMenu()
    {
        shopMenuManager.mainMenu.DisplayMainButtons(true);
        shopMenuManager.mainMenu.SetHeaderTMP("");
        shopMenuManager.mainMenu.DescriptionFieldClear();
        shopMenuManager.menuUpdateMethod = shopMenuManager.mainMenu;
        shopMenuManager.EnterMenu(shopMenuManager.mainMenu);
        shopMenuManager.mainMenu.mainShopMenuButtons[1].SetButtonNormalColor(Color.white);
    }

    public void OnInventorySlotSelected(InventorySlotUI inventorySlotUI)
    {
        if (inventorySlotUI.gearInstance.isCurrentlyEquipped)
            return;

        SellGear(inventorySlotUI);
    }

    void SellGear(InventorySlotUI inventorySlotUI)
    {
        var stats = shopMenuManager.mainMenu.playerPermanentStats;

        shopMenuManager.mainMenu.playerInventorySO.RemoveGearFromInventory(inventorySlotUI.gearInstance, true);
        InitialiseInventoryUI();

        int smamsInitialValue = Mathf.RoundToInt(currentlyDisplayedSmams);
        stats.Smams += inventorySlotUI.gearInstance.gearSO.Value;
        int smamsFinalValue = stats.Smams;

        if (smamsCoroutine != null)
            StopCoroutine(smamsCoroutine);

        smamsCoroutine = StartCoroutine(FieldEvents.LerpValuesCoRo(smamsInitialValue, smamsFinalValue, 0.2f, UpdateSmamsText));

        void UpdateSmamsText(float smamsValue)
        {
            currentlyDisplayedSmams = smamsValue;
            shopMenuManager.mainMenu.smamsInventoryTMP.text = "Account: " + Mathf.RoundToInt(smamsValue).ToString("N0") + " $MAMS";
        }

        shopMenuManager.mainMenu.smamsColorAnimator.Play("SmamsYellowText", 0, 0);
        shopMenuManager.sellMenu.InitialiseInventoryUI();

        var inventorySO = shopMenuManager.mainMenu.playerInventorySO;

        if (inventorySO.gearInstanceInventory.TrueForAll(x => x.gearSO == null))
        {
            ExitMenu();
            return;
        }

        highlightedButtonIndex = Mathf.Clamp(highlightedButtonIndex, 0, inventorySlots.Count - 1);

        firstButtonToSelect = inventorySlots[highlightedButtonIndex].button;
        firstButtonToSelect.Select();
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
                shopMenuManager.mainMenu.playerInventorySO.UnequipGear(inventorySlots[highlightedButtonIndex].gearInstance);
                InitialiseInventoryUI();
                inventorySlots[highlightedButtonIndex].button.Select();
            }
        }
    }
}
