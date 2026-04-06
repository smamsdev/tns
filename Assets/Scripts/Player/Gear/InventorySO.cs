using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]

public class InventorySO : ScriptableObject
{
    [Header("Debug")]
    public GearSO debugGearToAddAsInstance;

    [SerializeReference]
    public List<GearInstance> gearInstanceInventory = new List<GearInstance>();

    [Header("For Player")]
    [SerializeReference]
    public List<GearInstance> gearInstanceEquipped = new List<GearInstance>();
    public List<TrenchStructureSO> trenchStructuresInventory = new List<TrenchStructureSO>();

    void AddEquipSlot()
    {
        gearInstanceEquipped.Add(new GearInstance());
    }

    public bool AttemptAddGearToInventory(GearInstance gearInstanceToAdd, bool isSorted)
    {
        if (gearInstanceToAdd is EquipmentInstance equipmentInstance)
        {
            for (int i = 0; i < gearInstanceInventory.Count; i++)
            {
                if (gearInstanceInventory[i] == null)
                {
                    gearInstanceInventory[i] = equipmentInstance;
                    if (isSorted) SortInventory();

                    return true;
                }
            }

            return false;
        }

        else if (gearInstanceToAdd is ConsumableInstance consumableInstance)
        {
            Debug.Log("consumable detected");

            for (int i = 0; i < gearInstanceInventory.Count; i++)
            {
                if (gearInstanceInventory[i].gearSO == consumableInstance.gearSO)
                {
                    Debug.Log("match detected");
                    ConsumableInstance existingConsumableInstance = gearInstanceInventory[i] as ConsumableInstance;

                    //if the stack limit is not exceeded, +1 to existing stack
                    if (existingConsumableInstance.quantityAvailable < 9)
                    {
                        Debug.Log("available to add to stack");
                        existingConsumableInstance.quantityAvailable++;
                        return true;
                    }
                }
            }

            //if no stacks are detected, try to create a fresh stack
            for (int i = 0; i < gearInstanceInventory.Count; i++)
            {
                if (gearInstanceInventory[i] == null)
                {
                    gearInstanceInventory[i] = consumableInstance;
                    if (isSorted) SortInventory();
                    return true;
                }
            }

            return false;
        }

        else
        {
            Debug.Log("something went wrong");
            return false;
        }
    }

    public void SortInventory()
    {
        gearInstanceInventory.Sort((a, b) =>
        {
            if (a == null) return 1;
            if (b == null) return -1;

            return a.gearSO.gearName.CompareTo(b.gearSO.gearName);
        });
    }

    public void EquipGearToSlot(GearInstance gearInstanceToEquip, int equipSlotNumber)
    {
        if (gearInstanceEquipped[equipSlotNumber].gearSO != null)
        {
            UnequipGearFromSlot(gearInstanceEquipped[equipSlotNumber]);
        }

        gearInstanceToEquip.isCurrentlyEquipped = true;

        if (gearInstanceToEquip is ConsumableInstance consumableInstance)
        {
            consumableInstance.quantityAvailable--;
            gearInstanceEquipped[equipSlotNumber] = consumableInstance;
        }

        else if (gearInstanceToEquip is EquipmentInstance equipmentInstance)
        {
            gearInstanceEquipped[equipSlotNumber] = gearInstanceToEquip;
        }

        else
            Debug.Log("something went wrong");
    }

    public void UnequipGearFromSlot(GearInstance gearInstanceToUnequip)
    {
        gearInstanceToUnequip.isCurrentlyEquipped = false;
        int i = gearInstanceEquipped.IndexOf(gearInstanceToUnequip);
        gearInstanceEquipped[i] = new GearInstance();

        if (gearInstanceToUnequip is ConsumableInstance consumableInstance)
            consumableInstance.quantityAvailable++;
    }

    public void GearConsumed(GearSO gearToUnequip)
    {
        //gearToUnequip.isCurrentlyEquipped = false;
        //int index = inventorySO.equippedGear.IndexOf(gearToUnequip);
        //inventorySO.equippedGear[index] = null;

        Debug.Log("fix");
    }

    public void RemoveGearFromInventory(GearInstance gearInstanceToRemove, bool isSorted)
    {
        if (gearInstanceToRemove is EquipmentInstance equipmentInstance)
        {
            gearInstanceInventory[gearInstanceInventory.IndexOf(equipmentInstance)] = null;
            if (isSorted) SortInventory();
        }

        else if (gearInstanceToRemove is ConsumableInstance consumableInstance)
        {
            consumableInstance.quantityAvailable--;

            if (consumableInstance.quantityAvailable <= 0)
                gearInstanceInventory[gearInstanceInventory.IndexOf(consumableInstance)] = null;

            if (isSorted) SortInventory();
        }

        else
            Debug.Log("something went wrong");
    }
}