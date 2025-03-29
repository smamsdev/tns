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

        foreach (string inventoryString in inventorySO.equipSlotString)
        {
            if (string.IsNullOrEmpty(inventoryString))
            {
                equippedInventory.Add(null);
                continue;
            }

            Transform gearTransform = GearParent.Find(inventoryString);
            Gear gearToLoad = gearTransform.GetComponent<Gear>();
            gearToLoad.isCurrentlyEquipped = true;
            equippedInventory.Add(gearToLoad);
        }
    }

    public void AddGearToInventory(Gear gear)
    {
        inventorySO.inventoryString.Add(gear.name);
        inventorySO.inventoryString.Sort();
        inventory.Add(gear);
        gear.quantityInInventory++;
    }

    public void RemoveGearFromInventory(Gear gear)
    {
        inventorySO.inventoryString.Remove(gear.name);
        inventory.Remove(gear);
    }

    public void EquipGearToSlot(Gear gearToEquip, int slotNumber)
    {
        if (equippedInventory[slotNumber] != null)
        {
            UnequipGearFromSlot(equippedInventory[slotNumber], slotNumber);
        }

        gearToEquip.isCurrentlyEquipped = true;
        gearToEquip.quantityInInventory--;
        inventorySO.equipSlotString[slotNumber] = gearToEquip.name;
        equippedInventory[slotNumber] = gearToEquip;
    }

    public void UnequipGearFromSlot(Gear gearToUnequip, int slotNumber)
    {
        inventorySO.equipSlotString[slotNumber] = null;
        equippedInventory[slotNumber] = null;

        gearToUnequip.isCurrentlyEquipped = false;
        AddGearToInventory(gearToUnequip);
    }
}