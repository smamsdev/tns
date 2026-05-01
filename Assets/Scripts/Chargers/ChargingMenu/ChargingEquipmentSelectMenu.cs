using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargingEquipmentSelectMenu : ChargingMenu
{
    public GameObject inventorySlotUIPrefab, inventorySlotUIsParent, propertiesGO;
    public ChargingMainMenu chargingMainMenu;
    public ChargingSlotMenu chargingSlotMenu;
    public TextMeshProUGUI gearDescriptionTMP, gearQuantityTMP, gearEquipStatusTMP, headerTMP;

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
        var inventorySO = chargingMainMenu.playerInventorySO;
        if (inventorySO.gearInstanceInventory.TrueForAll(x => x.gearSO == null))
        {
            chargingMenuManager.menuUpdateMethod = chargingMenuManager.chargingMainMenu;
            return;
        }


        chargingMainMenu.menuButtonHighlighteds[1].SetButtonNormalColor(Color.yellow);
        chargingMainMenu.menuButtonHighlighteds[1].button.interactable = false;

        propertiesGO.SetActive(true);

        inventorySlotUIs[0].button.Select();
    }

    public void InitialiseInventoryUI()
    {
        DeleteAllInventoryUI();
        ClearText();

        var inventorySO = chargingMainMenu.playerInventorySO;

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

            if (i < inventorySO.gearInstanceInventory.Count && inventorySO.gearInstanceInventory[i].gearSO != null)
            {
                var gearInstance = inventorySO.gearInstanceInventory[i];

                inventorySlotUI.gearInstance = gearInstance;
                inventorySlotUI.itemNameTMP.text = gearInstance.gearSO.GearName;
                inventorySlotUI.itemQuantityTMP.text = inventorySlotUI.gearInstance.QuantityString();
                inventorySlotUI.name = "gear slot " + i + "" + inventorySlotUI.gearInstance.gearSO.GearName;

                bool isEquipment = gearInstance.gearSO is EquipmentSO;

                inventorySlotUI.icon.sprite = isEquipment ? inventorySlotUI.equipmentIcon : inventorySlotUI.consumableIcon;

                bool isChargeable = !gearInstance.isCurrentlyEquipped && gearInstance is EquipmentInstance && ((EquipmentInstance)gearInstance).ChargePercentage() != 100;
                
                FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.white, isChargeable ? 1 : .5f);
                FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.white, isChargeable ? 1 : .5f);

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
        FieldEvents.SetTextColor(inventorySlot.itemNameTMP, normalColor, inventorySlot.itemNameTMP.alpha);
        FieldEvents.SetTextColor(inventorySlot.itemQuantityTMP, normalColor, inventorySlot.itemQuantityTMP.alpha);
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
        gearQuantityTMP.text = "";
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

        chargingMenuManager.EnterMenu(chargingMenuManager.ChargingSlotSelectMenu);
        chargingMenuManager.DisplaySubMenu(chargingMenuManager.ChargingSlotSelectMenu);
        chargingMenuManager.ChargingSlotSelectMenu.chargingSlotUIs[chargingMenuManager.ChargingSlotSelectMenu.slotSelectedIndex].button.Select();
        chargingMenuManager.ChargingSlotSelectMenu.chargingSlotUIs[chargingMenuManager.ChargingSlotSelectMenu.slotSelectedIndex].onHighlighted.Invoke();
    }

    public void OnInventorySlotHighlighted(InventorySlotUI inventorySlotUI)
    {
        SetInventorySlotColor(inventorySlotUI, Color.yellow);
        highlightedButtonIndex = inventorySlotUIs.IndexOf(inventorySlotUI);

        var gi = inventorySlotUI.gearInstance;

        gearDescriptionTMP.text = "Description: " + gi.gearSO.GearDescription;

        EquipmentInstance equipment = null;

        switch (gi)
        {
            case EquipmentInstance equipmentInstance:
                equipment = equipmentInstance;
                gearQuantityTMP.text = equipmentInstance.ChargeTotalString();
                break;

            case ConsumableInstance:
                gearQuantityTMP.text = gi.QuantityString();
                gearEquipStatusTMP.text = "";
                headerTMP.text = "CONSUMABLE unable to charge";
                return;
        }

        if (gi.isCurrentlyEquipped)
        {
            headerTMP.text = "GEAR is equipped to Slot " +
                (chargingMainMenu.playerInventorySO.gearInstanceEquipped.IndexOf(gi) + 1);

            gearEquipStatusTMP.text = gearEquipStatusTMP.text = "Equipped to Slot " + (gi.EquippedSlotInt(chargingMainMenu.playerInventorySO) + 1) + ". Press CTRL to unequip";
            return;
        }

        if (equipment.ChargePercentage() == 100)
        {
            headerTMP.text = gi.gearSO.GearName + " at max charge";
            gearEquipStatusTMP.text = "";
            return;
        }

        headerTMP.text = "Charge " + gi.gearSO.GearName + "?";
        gearEquipStatusTMP.text = "";
    }

    public void UnequipHighlightedGearInstance(GearInstance gearInstance)
    {
        int i = chargingMainMenu.playerInventorySO.gearInstanceEquipped.IndexOf(gearInstance);
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
                chargingMainMenu.playerInventorySO.UnequipGear(inventorySlotUIs[highlightedButtonIndex].gearInstance);
                InitialiseInventoryUI();
                inventorySlotUIs[highlightedButtonIndex].button.Select();
            }
        }
    }
}
