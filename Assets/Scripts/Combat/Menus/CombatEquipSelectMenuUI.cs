using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatEquipSelectMenuUI : CombatMenu
{
    public CombatManager combatManager;
    public GameObject UIInventorySlotPrefab, equipSlotsParent;
    public List<InventorySlotUI> equipSlots = new List<InventorySlotUI>();

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

                equipSlot.itemQuantityTMP.text = equipSlot.gearInstance.QuantityString();

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

        List<Button> equipSlotButtons = new List<Button>();
        foreach (var equipSlot in equipSlots)
            equipSlotButtons.Add(equipSlot.button);

        FieldEvents.SetGridNavigationWrapAround(equipSlotButtons, gearInstanceEquipped.Count);
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

