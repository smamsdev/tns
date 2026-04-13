using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargingEquipmentSelectMenu : ChargingMenu
{
    public GameObject inventorySlotUIPrefab, inventorySlotUIsParent, propertiesGO;
    public ChargingMainMenu chargingMainMenu;
    public ChargingSlotMenu chargingSlotMenu;
    public TextMeshProUGUI gearDescriptionTMP, gearTypeTMP, gearEquipStatusTMP;

    [Header("Dynamic")]

    public List<InventorySlotUI> inventorySlotUIs = new List<InventorySlotUI>();
    public int highlightedButtonIndex;
    public EquipmentInstance equipmentInstanceToCharge;

    public override void DisplayMenu(bool on)
    {
        displayContainer.SetActive(on);
    }

    public override void EnterMenu()
    {
        var inventorySO = chargingMainMenu.playerInventory.inventorySO;
        if (inventorySO.gearInstanceInventory.TrueForAll(x => x == null))
            return;

        chargingMainMenu.menuButtonHighlighteds[1].SetButtonNormalColor(Color.yellow);
        chargingMainMenu.menuButtonHighlighteds[1].button.interactable = false;

        propertiesGO.SetActive(true);

        inventorySlotUIs[0].button.Select();
    }

    public void InitialiseInventoryUI()
    {
        DeleteAllInventoryUI();
        ClearText();

        var inventorySO = chargingMainMenu.playerInventory.inventorySO;

        for (int i = 0; i < inventorySO.gearInstanceInventory.Count; i++)
        {
            GameObject UIgearSlotGO = Instantiate(inventorySlotUIPrefab);
            UIgearSlotGO.transform.SetParent(inventorySlotUIsParent.transform, false);

            UIgearSlotGO.name = "gear slot " + i;
            InventorySlotUI inventorySlotUI = UIgearSlotGO.GetComponent<InventorySlotUI>();

            inventorySlotUI.itemNameTMP.text = "";
            inventorySlotUI.itemQuantityTMP.text = "";
            inventorySlotUI.icon.sprite = inventorySlotUI.freeIcon;
            inventorySlotUI.name = "gear slot " + i;

            if (i < inventorySO.gearInstanceInventory.Count && inventorySO.gearInstanceInventory[i] != null)
            {
                var gearInstance = inventorySO.gearInstanceInventory[i];

                inventorySlotUI.gearInstance = gearInstance;
                inventorySlotUI.itemNameTMP.text = gearInstance.gearSO.gearName;
                inventorySlotUI.itemQuantityTMP.text = FieldEvents.ItemQuantityRemaining(inventorySlotUI.gearInstance);
                inventorySlotUI.name = "gear slot " + i + "" + inventorySlotUI.gearInstance.gearSO.gearName;

                bool isEquipment = gearInstance.gearSO is EquipmentSO;

                inventorySlotUI.icon.sprite = isEquipment ? inventorySlotUI.equipmentIcon : inventorySlotUI.consumableIcon;

                bool isCurrentlyEquipped = gearInstance.isCurrentlyEquipped;
                FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.white, isCurrentlyEquipped ? .7f : 1);
                FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.white, isCurrentlyEquipped ? .7f : 1);

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

                inventorySlotUIs.Add(inventorySlotUI);
            }
        }

        List<Button> inventorySlotButtons = new List<Button>();
        foreach (var inventorySlot in inventorySlotUIs)
            inventorySlotButtons.Add(inventorySlot.button);

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

    public void SetInventorySlotColor(InventorySlotUI inventorySlot, Color normalColor)
    {
        float alpha = 0.7f;

        if (inventorySlot.gearInstance is EquipmentInstance && ((EquipmentInstance)inventorySlot.gearInstance).ChargePercentage() != 100)
            alpha = 1f;

        FieldEvents.SetTextColor(inventorySlot.itemNameTMP, normalColor, alpha);
        FieldEvents.SetTextColor(inventorySlot.itemQuantityTMP, normalColor, alpha);
    }

    public void DeleteAllInventoryUI()
    {
        inventorySlotUIs.Clear();

        for (int i = inventorySlotUIsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventorySlotUIsParent.transform.GetChild(i).gameObject);
        }
    }

    void ClearText()
    {
        gearDescriptionTMP.text = "";
        gearTypeTMP.text = "";
        gearEquipStatusTMP.text = "";
    }

    public void OnInventorySlotSelected(InventorySlotUI inventorySlot)
    {
        if (inventorySlot.gearInstance.isCurrentlyEquipped || inventorySlot.gearInstance is ConsumableInstance || ((EquipmentInstance)inventorySlot.gearInstance).ChargePercentage() == 100) 
            return;

        highlightedButtonIndex = inventorySlotUIs.IndexOf(inventorySlot);

        equipmentInstanceToCharge = inventorySlot.gearInstance as EquipmentInstance;

        chargingSlotMenu.isEquipping = true;
        ExitMenu();

        chargingSlotMenu.pageHeaderTMP.text = "Start charging " + inventorySlot.gearInstance.gearSO.gearName + "?";
        chargingMenuManager.EnterMenu(chargingMenuManager.ChargingSlotSelectMenu);

        chargingMenuManager.DisplaySubMenu(chargingMenuManager.ChargingSlotSelectMenu);
        chargingMenuManager.ChargingSlotSelectMenu.chargingSlotUIs[0].button.Select();
        chargingMenuManager.ChargingSlotSelectMenu.chargingSlotUIs[0].onHighlighted();
    }

    public void OnInventorySlotHighlighted(InventorySlotUI inventorySlotUI)
    {
        SetInventorySlotColor(inventorySlotUI, Color.yellow);

        highlightedButtonIndex = inventorySlotUIs.IndexOf(inventorySlotUI);

        gearDescriptionTMP.text = "Description: " + inventorySlotUI.gearInstance.gearSO.gearDescription;

        //Gear Type
        if (inventorySlotUI.gearInstance is EquipmentInstance equipmentInstance)
        { 
            gearTypeTMP.text = "Charge " + equipmentInstance.Charge + " / " + ((EquipmentSO)equipmentInstance.gearSO).maxPotential;
        }

        else
            gearTypeTMP.text = "Consumable";

        //Availability

        if (inventorySlotUI.gearInstance is ConsumableInstance)
            gearEquipStatusTMP.text = "";

        else
        {
            if (inventorySlotUI.gearInstance.isCurrentlyEquipped)
            {
                gearEquipStatusTMP.text = "Equipped to Slot " + (chargingMainMenu.playerInventory.inventorySO.gearInstanceEquipped.IndexOf(inventorySlotUI.gearInstance) + 1) + ". PRESS CTRL TO REMOVE";
            }

            if (((EquipmentInstance)inventorySlotUI.gearInstance).ChargePercentage() == 100)
            {
                gearEquipStatusTMP.text = "Fully charged";
            }

            else
            {
                gearEquipStatusTMP.text = "SELECT to Charge";
            }
        }
    }

    public void UnequipHighlightedGearInstance(GearInstance gearInstance)
    {
        int i = chargingMainMenu.playerInventory.inventorySO.gearInstanceEquipped.IndexOf(gearInstance);

        chargingMainMenu.playerInventory.inventorySO.UnequipGearFromSlot(chargingMainMenu.playerInventory.inventorySO.gearInstanceEquipped[i]);
        InitialiseInventoryUI();
    }

    public override void ExitMenu()
    {
        ClearText();

        chargingMenuManager.ChargingSlotSelectMenu.pageHeaderTMP.text = "Select Charging Slot:";

        if (inventorySlotUIs.Count > 0)
            inventorySlotUIs[highlightedButtonIndex].onUnHighlighted();

        chargingMainMenu.menuButtonHighlighteds[1].button.interactable = true;
        chargingMainMenu.menuButtonHighlighteds[1].SetButtonNormalColor(Color.white);
        chargingMenuManager.chargingMainMenu.firstButtonToSelect = chargingMenuManager.chargingMainMenu.menuButtonHighlighteds[1].button;

        chargingMenuManager.EnterMenu(chargingMenuManager.chargingMainMenu);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (inventorySlotUIs[highlightedButtonIndex].gearInstance.isCurrentlyEquipped)
            {
                DisplayMenu(true);
                UnequipHighlightedGearInstance(inventorySlotUIs[highlightedButtonIndex].gearInstance);
                inventorySlotUIs[highlightedButtonIndex].button.Select();
            }
        }
    }
}
