using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatGearSelectMenuUI : CombatMenu
{
    public CombatManager combatManager;
    PlayerInventorySO playerInventorySO;
    public CombatGearState combatGearState;

    public GameObject inventorySlotUIPrefab, inventorySlotsParent;
    public List<InventorySlotUI> inventorySlotUIs = new();

    public void InitialiseInventoryUI()
    {
        var inventorySO = combatManager.playerCombat.playerInventorySO;

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
                bool isCurrentlyEquipped = gearInstance.isCurrentlyEquipped;

                inventorySlotUI.icon.sprite = isEquipment ? inventorySlotUI.equipmentIcon : inventorySlotUI.consumableIcon;

                if (!isCurrentlyEquipped)
                    inventorySlotUI.button.onClick.AddListener(() => combatGearState.OnInventorySlotSelected(inventorySlotUI));

                if (isCurrentlyEquipped)
                    menuManager.SetGearSlotUIColor(inventorySlotUI, Color.white, .7f);

                    inventorySlotUI.onHighlighted = () =>
                    OnInventorySlotHighlighted(inventorySlotUI);

                inventorySlotUI.onUnHighlighted = () =>
                    OnInventorySlotUnhighlighted(inventorySlotUI);

                inventorySlotUIs.Add(inventorySlotUI);
            }
        }

        List<Button> inventorySlotButtons = new();

        foreach (var inventorySlot in inventorySlotUIs)
            if (inventorySlot.gearInstance != null) inventorySlotButtons.Add(inventorySlot.button);

        FieldEvents.SetGridNavigationWrapAroundHorizontal(inventorySlotButtons, 3);
    }

    public void DeleteAllInventoryUI()
    {
        inventorySlotUIs.Clear();

        for (int i = inventorySlotsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventorySlotsParent.transform.GetChild(i).gameObject);
        }
    }

    public void OnInventorySlotHighlighted(InventorySlotUI inventorySlot)
    {
        menuManager.SetGearSlotUIColor(inventorySlot, Color.yellow, inventorySlot.itemNameTMP.alpha);
        menuManager.UpdateNarrator(inventorySlot.gearInstance.gearSO.GearDescription);

        GearInstance gearInstance = inventorySlot.gearInstance;

        if (gearInstance.isCurrentlyEquipped)
        {
            int slotNumber = gearInstance.EquippedSlotInt(combatManager.playerCombat.playerInventorySO);
            menuManager.UpdateNarrator(gearInstance.gearSO.GearDescription + "\nEquipped to slot " + slotNumber + ". Press CTRL to unequip");
        }

    }

    public void OnInventorySlotUnhighlighted(InventorySlotUI inventorySlot)
    {
        menuManager.SetGearSlotUIColor(inventorySlot, Color.white, inventorySlot.itemNameTMP.alpha);
    }
}
