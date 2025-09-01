using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventorySO inventorySO;

        public void AddGearToInventory(GearSO gear)
        {
            inventorySO.gearInventory.Add(gear);
        }
        
        public void RemoveGearFromInventory(GearSO gear)
        {
            inventorySO.gearInventory.Remove(gear);
        }
        
        public void EquipGearToSlot(GearSO gearSOToEquip, int equipSlotNumber)
        {
            if (inventorySO.equippedGear[equipSlotNumber] != null)
            {
                UnequipGearFromSlot(inventorySO.equippedGear[equipSlotNumber]);
            }

            gearSOToEquip.isCurrentlyEquipped = true;
            gearSOToEquip.equipSlotNumber = equipSlotNumber;

            inventorySO.equippedGear[equipSlotNumber] = gearSOToEquip;
        }
        
        public void UnequipGearFromSlot(GearSO gearToUnequip)
        {
            inventorySO.equippedGear[gearToUnequip.equipSlotNumber] = null;
            gearToUnequip.isCurrentlyEquipped = false;
            gearToUnequip.equipSlotNumber = -1;
        }
}