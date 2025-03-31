using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventorySO inventorySO;
    public List<Gear> inventory = new List<Gear>();
    public List <Gear> equippedInventory = new List<Gear>();

    public Transform GearParent;

    private void Start()
    {
        LoadInventoryFromSO();
    }

    private void SaveInventory()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (!inventorySO.inventoryString.Contains(inventory[i].gearID))
                inventorySO.inventoryString.Add(inventory[i].gearID);
        }
    }

    public void LoadInventoryFromSO()
    {
        inventorySO.inventoryString.Sort();
        inventory.Clear();

        for (int i = 0; i < inventorySO.inventoryString.Count; i++)
        {
            Transform gearTransform = GearParent.Find(inventorySO.inventoryString[i]);

            if (gearTransform != null)
            {
                Gear gearToLoad = gearTransform.GetComponent<Gear>();
                gearToLoad.isCurrentlyEquipped = false;
                gearToLoad.quantityInInventory++;

                if (!inventory.Contains(gearToLoad))
                {
                    gearToLoad.quantityInInventory = 1;
                    inventory.Add(gearToLoad);
                }
            }
            else
            {
                Debug.LogWarning("Gear not found in scene: " + inventorySO.inventoryString[i]);
            }
        }

        LoadEquippedSlotsFromSO();
    }

    public void LoadEquippedSlotsFromSO()
    {
        equippedInventory.Clear();

        for (int i = 0; i < inventorySO.equipSlotString.Count; i++)
        {
            string inventoryString = inventorySO.equipSlotString[i];

            if (string.IsNullOrEmpty(inventoryString))
            {
                equippedInventory.Add(null);
                continue;
            }

            Transform gearTransform = GearParent.Find(inventoryString);
            Gear gearToLoad = gearTransform.GetComponent<Gear>();
            gearToLoad.isCurrentlyEquipped = true;
            gearToLoad.equipSlotNumber = i;
            equippedInventory.Add(gearToLoad);
        }
    }

    public void AddGearToInventory(Gear gear)
    {
        inventorySO.inventoryString.Add(gear.name);
        LoadInventoryFromSO();
    }

    public void RemoveGearFromInventory(Gear gear)
    {
        inventorySO.inventoryString.Remove(gear.name);
        LoadInventoryFromSO();
    }

    public void EquipGearToSlot(Gear gearToEquip, int equipSlotNumber)
    {
        if (equippedInventory[equipSlotNumber] != null)
        {
            UnequipGearFromSlot(equippedInventory[equipSlotNumber]);
        }

        gearToEquip.isCurrentlyEquipped = true;
        gearToEquip.equipSlotNumber = equipSlotNumber;
        inventorySO.equipSlotString[equipSlotNumber] = gearToEquip.name;
        equippedInventory[equipSlotNumber] = gearToEquip;

        LoadEquippedSlotsFromSO();
    }

    public void UnequipGearFromSlot(Gear gearToUnequip)
    {
        inventorySO.equipSlotString[gearToUnequip.equipSlotNumber] = null;
        gearToUnequip.isCurrentlyEquipped = false;
        gearToUnequip.equipSlotNumber = -1;
        LoadEquippedSlotsFromSO();
    }
}