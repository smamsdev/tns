using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatEquipSelectMenuUI : CombatMenu
{
    public CombatManager combatManager;
    public EquipSlotSelectState equipSlotSelectState;
    public GameObject UIInventorySlotPrefab, equipSlotsParent;
    public List<InventorySlotUI> equipSlots = new List<InventorySlotUI>();
    public GridLayoutGroup slotGridLayoutGroup;

    public void InitialiseEquipSlots()
    {
        DeleteAllInventoryUI();

        var gearInstanceEquipped = combatManager.playerCombat.playerInventorySO.gearInstanceEquipped;

        for (int i = 0; i < gearInstanceEquipped.Count; i++)
        {
            GameObject UIEquipSlotGO = Instantiate(UIInventorySlotPrefab);
            UIEquipSlotGO.transform.SetParent(equipSlotsParent.transform, false);
            UIEquipSlotGO.name = "Equip slot " + (i + 1);

            InventorySlotUI equipSlot = UIEquipSlotGO.GetComponent<InventorySlotUI>();

            equipSlot.icon.sprite = equipSlot.equipmentIcon;

            if (gearInstanceEquipped[i] == null || gearInstanceEquipped[i].gearSO == null)
            {
                equipSlot.itemNameTMP.text = "Slot Available";
                equipSlot.itemQuantityTMP.text = "";
                equipSlot.icon.sprite = equipSlot.freeIcon;
                equipSlot.gearInstance = new GearInstance();
                menuManager.SetGearSlotUIColor(equipSlot, Color.white, 1f);

                equipSlot.button.onClick.AddListener(() => equipSlotSelectState.OnEquipSlotSelected(equipSlot));
            }

            else
            {
                equipSlot.gearInstance = gearInstanceEquipped[i];
                equipSlot.itemNameTMP.text = equipSlot.gearInstance.gearSO.GearName;

                if (gearInstanceEquipped[i] is EquipmentInstance)
                    equipSlot.itemQuantityTMP.text = equipSlot.gearInstance.QuantityString();

                // you can only ever equip a single instance of a consumable, so you might as well hide this
                else
                    equipSlot.itemQuantityTMP.text = "";

                bool isEquipment = equipSlot.gearInstance is EquipmentInstance;
                equipSlot.icon.sprite = isEquipment ? equipSlot.equipmentIcon : equipSlot.consumableIcon;

                menuManager.SetGearSlotUIColor(equipSlot, Color.white, .7f);
            }

            equipSlot.onHighlighted = () =>
            {
                OnEquipSlotHighlighted(equipSlot);
                menuManager.SetGearSlotUIColor(equipSlot, Color.yellow, equipSlot.itemNameTMP.alpha);
            };

            equipSlot.onUnHighlighted = () =>
            {
                menuManager.SetGearSlotUIColor(equipSlot, Color.white, equipSlot.itemNameTMP.alpha);
            };

            equipSlots.Add(equipSlot);
        }

        InventorySlotUI longestSlot = FindSlotWithLongestText(equipSlots);
        Vector2 newCellSize = slotGridLayoutGroup.cellSize;
        int padding = 75;
        float newWidth = longestSlot.itemNameTMP.preferredWidth + longestSlot.itemQuantityTMP.preferredWidth + padding;

        newCellSize = new Vector2(newWidth, newCellSize.y);
        slotGridLayoutGroup.cellSize = newCellSize;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(slotGridLayoutGroup.cellSize.x, rectTransform.sizeDelta.y);

        foreach (var equipSlot in equipSlots)
            menuButtons.Add(equipSlot.button);

        FieldEvents.SetGridNavigationWrapAround(menuButtons, gearInstanceEquipped.Count);
    }

    InventorySlotUI FindSlotWithLongestText(List<InventorySlotUI> slots)
    {
        return slots.OrderByDescending(slot => slot.itemNameTMP.preferredWidth + slot.itemQuantityTMP.preferredWidth).First();
    }

    public void DeleteAllInventoryUI()
    {
        equipSlots.Clear();
        menuButtons.Clear();

        for (int i = equipSlotsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(equipSlotsParent.transform.GetChild(i).gameObject);
        }
    }

    public void OnEquipSlotHighlighted(InventorySlotUI inventorySlotUI)
    {
        GearSO gearSO = inventorySlotUI.gearInstance.gearSO;
        highlightedButtonIndex = equipSlots.IndexOf(inventorySlotUI);

        if (gearSO == null)
        {
            menuManager.UpdateNarrator("Equip GEAR to Slot " + (highlightedButtonIndex + 1) + "?");
            return;
        }

        menuManager.UpdateNarrator(inventorySlotUI.gearInstance.gearSO.GearDescription + "\nPress CTRL to unequip");
    }
}

