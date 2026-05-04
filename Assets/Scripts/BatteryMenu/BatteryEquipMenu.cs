using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryEquipMenu : BatteryMenu
{
    public GameObject inventorySlotUIPrefab, inventorySlotsParent;
    public List<InventorySlotUI> inventorySlots = new();
    public InventorySlotUI batterySlotUI;
    public int highlightedButtonIndex = 0;
    public bool isGearSelectMode;

    public override void DisplayMenu(bool on)
    {
    }

    public override void EnterMenu()
    {
        batteryMenuManager.batteryMainMenu.DisplayMainButtons(false);
        batterySlotUI.button.Select();
    }

    public override void ExitMenu()
    {
        throw new System.NotImplementedException();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!isGearSelectMode && batterySlotUI.gearInstance.gearSO != null)
            {
                InventorySO batteryInstanceSO = batteryMenuManager.batteryMainMenu.vehicleInstance.batteryInventorySO;
                PlayerInventorySO playerInventorySO = batteryMenuManager.batteryMainMenu.playerInventorySO;

                if (playerInventorySO.AttemptAddGearToInventory(batteryInstanceSO.gearInstanceInventory[0], true))
                {
                    batteryInstanceSO.gearInstanceInventory[0] = new EquipmentInstance();
                    InitialiseBatterySlot();
                    InitialiseInventoryUI();
                    batterySlotUI.onHighlighted.Invoke();
                } 
            }

            if (inventorySlots[highlightedButtonIndex].gearInstance.isCurrentlyEquipped)
            {
                UnequipHighlightedGearInstance(inventorySlots[highlightedButtonIndex].gearInstance);
                inventorySlots[highlightedButtonIndex].button.Select();
            }
        }
    }

    public void InitialiseBatterySlot()
    {
        GearInstance batteryInstanceSO = batteryMenuManager.batteryMainMenu.vehicleInstance.batteryInventorySO.gearInstanceInventory[0];

        batterySlotUI.gearInstance = new EquipmentInstance();

        batterySlotUI.itemNameTMP.text = "Empty";
        batterySlotUI.itemQuantityTMP.text = "";

        if (batteryInstanceSO.gearSO != null)
        {
            batterySlotUI.gearInstance = batteryInstanceSO;
            batterySlotUI.itemNameTMP.text = batteryInstanceSO.gearSO.GearName;
            batterySlotUI.itemQuantityTMP.text = batteryInstanceSO.QuantityString();
        }

        batterySlotUI.button.onClick.AddListener(OnBatterySlotSelected);

        batterySlotUI.onHighlighted = () => BatterySlotHighlighted(batterySlotUI);

        batterySlotUI.onUnHighlighted = () =>
        {
            FieldEvents.SetTextColor(batterySlotUI.itemNameTMP, Color.white, batterySlotUI.itemNameTMP.alpha);
            FieldEvents.SetTextColor(batterySlotUI.itemQuantityTMP, Color.white, batterySlotUI.itemNameTMP.alpha);
        };
    }

    void OnBatterySlotSelected()
    {
        isGearSelectMode = true;

        foreach (InventorySlotUI inventorySlotUI in inventorySlots)
        {
            bool isAvailable = !inventorySlotUI.gearInstance.isCurrentlyEquipped && inventorySlotUI.gearInstance.gearSO is BatterySO;
            SetGearSlotAlpha(inventorySlotUI, isAvailable, 1, 0.5f);
        }
        
        inventorySlots[0].button.Select();
    }

    void BatterySlotHighlighted(InventorySlotUI batterySlotUI)
    {
        FieldEvents.SetTextColor(batterySlotUI.itemNameTMP, Color.yellow, batterySlotUI.itemNameTMP.alpha);
        FieldEvents.SetTextColor(batterySlotUI.itemQuantityTMP, Color.yellow, batterySlotUI.itemNameTMP.alpha);

        if (batterySlotUI.gearInstance.gearSO != null)
        {
            batteryMenuManager.batteryMainMenu.UpdateDescriptionDisplayTMPs(batterySlotUI.gearInstance);
            batteryMenuManager.batteryMainMenu.gearEquipStatusTMP.text = "Press CTRL to retrieve from vehicle";
            batteryMenuManager.batteryMainMenu.headerTMP.text = "Replace " + batterySlotUI.gearInstance.gearSO.GearName + "?";
            return;
        }

        batteryMenuManager.batteryMainMenu.ClearAllDescriptionTMPs();
        batteryMenuManager.batteryMainMenu.headerTMP.text = "Select a Battery";
    }

    public void UnequipHighlightedGearInstance(GearInstance gearInstance)
    {
        batteryMenuManager.batteryMainMenu.playerInventorySO.UnequipGear(gearInstance);
        InitialiseInventoryUI();

        foreach (InventorySlotUI inventorySlotUI in inventorySlots)
        {
            bool isAvailable = !inventorySlotUI.gearInstance.isCurrentlyEquipped && inventorySlotUI.gearInstance.gearSO is BatterySO;
            SetGearSlotAlpha(inventorySlotUI, isAvailable, 1, 0.5f);
        }
    }

    public void SetGearSlotAlpha(InventorySlotUI inventorySlotUI, bool isAvailable, float alphaIfAvailable, float alphaIfUnavailable)
    {
        FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, inventorySlotUI.itemNameTMP.color, isAvailable ? alphaIfAvailable : alphaIfUnavailable);
        FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, inventorySlotUI.itemQuantityTMP.color, isAvailable ? alphaIfAvailable : alphaIfUnavailable);
    }

    public void InitialiseInventoryUI()
    {
        DeleteAllInventoryUI();

        var inventorySO = batteryMenuManager.batteryMainMenu.playerInventorySO;

        for (int i = 0; i < inventorySO.gearInstanceInventory.Count; i++)
        {
            GameObject UIgearSlotGO = Instantiate(inventorySlotUIPrefab);
            UIgearSlotGO.transform.SetParent(inventorySlotsParent.transform, false);

            //scale smaller than usual for this menu
            UIgearSlotGO.transform.localScale = new Vector3(.7f, .7f, .7f);
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

                FieldEvents.SetTextColor(inventorySlotUI.itemNameTMP, Color.white, .5f);
                FieldEvents.SetTextColor(inventorySlotUI.itemQuantityTMP, Color.white, .5f);

                bool isCurrentlyEquipped = gearInstance.isCurrentlyEquipped;

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

    public void DeleteAllInventoryUI()
    {
        inventorySlots.Clear();

        for (int i = inventorySlotsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventorySlotsParent.transform.GetChild(i).gameObject);
        }
    }

    public void OnInventorySlotSelected(InventorySlotUI inventorySlotUI)
    {
        bool isAvailable = !inventorySlotUI.gearInstance.isCurrentlyEquipped && inventorySlotUI.gearInstance.gearSO is BatterySO;

        if (!isAvailable)
            return;

        if (batterySlotUI.gearInstance.gearSO != null)
        {
            if (!batteryMenuManager.batteryMainMenu.playerInventorySO.AttemptAddGearToInventory(batterySlotUI.gearInstance, true))
            {
                batteryMenuManager.batteryMainMenu.headerTMP.text = "Inventory full, unable to retrieve " + batterySlotUI.gearInstance.gearSO.GearName;
                return;
            }
        }

        batteryMenuManager.batteryMainMenu.vehicleInstance.batteryInventorySO.gearInstanceInventory[0] = inventorySlotUI.gearInstance;
        batteryMenuManager.batteryEquipMenu.InitialiseBatterySlot();
        batteryMenuManager.batteryEquipMenu.InitialiseInventoryUI();
        batteryMenuManager.batteryMainMenu.ClearAllDescriptionTMPs();
        isGearSelectMode = false;
        batterySlotUI.onHighlighted.Invoke();
    }

    public void OnInventorySlotHighlighted(InventorySlotUI inventorySlot)
    {
        FieldEvents.SetTextColor(inventorySlot.itemNameTMP, Color.yellow, inventorySlot.itemNameTMP.alpha);
        FieldEvents.SetTextColor(inventorySlot.itemQuantityTMP, Color.yellow, inventorySlot.itemNameTMP.alpha);
        highlightedButtonIndex = inventorySlots.IndexOf(inventorySlot);
        batteryMenuManager.batteryMainMenu.UpdateDescriptionDisplayTMPs(inventorySlot.gearInstance);

        if (inventorySlot.gearInstance.gearSO is not BatterySO)
            batteryMenuManager.batteryMainMenu.headerTMP.text = "Select a Battery";

        else
            batteryMenuManager.batteryMainMenu.headerTMP.text = "Connect " + inventorySlot.gearInstance.gearSO.GearName + "?";
    }
}
