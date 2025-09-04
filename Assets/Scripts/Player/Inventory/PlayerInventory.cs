using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventorySO inventorySO;

    public void AddGearToInventory(GearSO gear)
    {
        inventorySO.gearInventory.Add(gear);
        gear.quantityInInventory++;
    }

    public void RemoveGearFromInventory(GearSO gear)
    {
        inventorySO.gearInventory.Remove(gear);
        gear.quantityInInventory++;
    }

    public void EquipGearToSlot(GearSO gearSOToEquip, int equipSlotNumber)
    {
        if (inventorySO.equippedGear[equipSlotNumber] != null)
        {
            UnequipGearFromSlot(inventorySO.equippedGear[equipSlotNumber]);
        }

        gearSOToEquip.isCurrentlyEquipped = true;
        gearSOToEquip.quantityInInventory--;
        inventorySO.equippedGear[equipSlotNumber] = gearSOToEquip;
    }
        
    public void UnequipGearFromSlot(GearSO gearToUnequip)
    {
        gearToUnequip.isCurrentlyEquipped = false;
        gearToUnequip.quantityInInventory++;
        int index = inventorySO.equippedGear.IndexOf(gearToUnequip);
        inventorySO.equippedGear[index] = null;
    }
}