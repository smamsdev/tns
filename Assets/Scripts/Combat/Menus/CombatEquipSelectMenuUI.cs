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

            equipSlot.button.onClick.AddListener(() => OnEquipSlotSelected(equipSlot));
            equipSlot.icon.sprite = equipSlot.equipmentIcon;

            if (gearInstanceEquipped[i] == null || gearInstanceEquipped[i].gearSO == null)
            {
                equipSlot.itemNameTMP.text = "Free";
                equipSlot.itemQuantityTMP.text = "";
                equipSlot.icon.sprite = equipSlot.freeIcon;
                equipSlot.gearInstance = new GearInstance();
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
            }

            equipSlot.onHighlighted = () =>
            {
                OnEquipSlotHighlighted(equipSlot);
            };

            equipSlot.onUnHighlighted = () =>
            {
                //menuGearMainPage.SetSlotColor(equipSlot, Color.white);
            };

            equipSlots.Add(equipSlot);
        }

        InventorySlotUI longestSlot = FindSlotWithLongestText(equipSlots);
        Vector2 newCellSize = slotGridLayoutGroup.cellSize;
        int padding = 60;
        float newWidth = longestSlot.itemNameTMP.preferredWidth + longestSlot.itemQuantityTMP.preferredWidth + padding;

        newCellSize = new Vector2(newWidth, newCellSize.y);
        slotGridLayoutGroup.cellSize = newCellSize;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(newWidth + slotGridLayoutGroup.padding.left, rectTransform.sizeDelta.y);

        foreach (var equipSlot in equipSlots)
            menuButtons.Add(equipSlot.button);

        FieldEvents.SetGridNavigationWrapAround(menuButtons, gearInstanceEquipped.Count);
    }

    InventorySlotUI FindSlotWithLongestText(List<InventorySlotUI> slots)
    {
        return slots.OrderByDescending(slot => slot.itemNameTMP.preferredWidth).First();
    }

    public void DeleteAllInventoryUI()
    {
        equipSlots.Clear();

        for (int i = equipSlotsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(equipSlotsParent.transform.GetChild(i).gameObject);
        }
    }

    public void OnEquipSlotHighlighted(InventorySlotUI inventorySlotUI)
    {
        //
    }

    public void OnEquipSlotSelected(InventorySlotUI gearEquipSlotSelected)
    {
        //
    }
}

