using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;

[CreateAssetMenu]

public class InventorySO : ScriptableObject
{
    [Header("Debug")]
    public GearSO debugGearToAddAsInstance;

    [SerializeReference]
    public List<GearInstance> gearInstanceInventory = new List<GearInstance>();

    public bool AttemptAddGearToInventory(GearInstance gearInstanceToAdd, bool isSorted)
    {
        if (gearInstanceToAdd is EquipmentInstance equipmentInstance)
        {
            for (int i = 0; i < gearInstanceInventory.Count; i++)
            {
                if (gearInstanceInventory[i].gearSO == null)
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
            // Debug.Log("consumable detected");

            for (int i = 0; i < gearInstanceInventory.Count; i++)
            {
                if (gearInstanceInventory[i].gearSO == consumableInstance.gearSO)
                {
                    // Debug.Log("match detected");
                    ConsumableInstance existingConsumableInstance = gearInstanceInventory[i] as ConsumableInstance;

                    //if the stack limit is not exceeded, +1 to existing stack
                    if (existingConsumableInstance.quantityAvailable < 3)
                    {
                        // Debug.Log("available to add to stack");
                        existingConsumableInstance.quantityAvailable++;
                        return true;
                    }
                }
            }

            //Debug.Log("attempt create fresh instance");
            //if no stacks are detected, try to create a fresh stack if there is inventory space
            for (int i = 0; i < gearInstanceInventory.Count; i++)
            {
                if (gearInstanceInventory[i].gearSO == null)
                {
                    //Debug.Log("create fresh instance");
                    gearInstanceInventory[i] = new ConsumableInstance(consumableInstance);
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

    public void RemoveGearFromInventory(GearInstance gearInstanceToRemove, bool isSorted)
    {
        if (gearInstanceToRemove is EquipmentInstance equipmentInstance)
        {
            gearInstanceInventory[gearInstanceInventory.IndexOf(equipmentInstance)] = new GearInstance();
            if (isSorted) SortInventory();
        }

        else if (gearInstanceToRemove is ConsumableInstance consumableInstance)
        {
            consumableInstance.quantityAvailable--;

            if (consumableInstance.quantityAvailable <= 0)
                gearInstanceInventory[gearInstanceInventory.IndexOf(consumableInstance)] = new GearInstance();

            if (isSorted) SortInventory();
        }

        else
            Debug.Log("something went wrong");
    }

    public void SortInventory()
    {
        MergeConsumableStacks();

        gearInstanceInventory.Sort((a, b) =>
        {
            if (a.gearSO == null) return 1;
            if (b.gearSO == null) return -1;

            int nameCompare = a.gearSO.gearName.CompareTo(b.gearSO.gearName);
            if (nameCompare != 0)
                return nameCompare;

            float aCharge = (a is EquipmentInstance aEquip) ? aEquip.ChargePercentage() : 0f;
            float bCharge = (b is EquipmentInstance bEquip) ? bEquip.ChargePercentage() : 0f;

            //sort charge by descending
            return bCharge.CompareTo(aCharge);
        });
    }

    public void MergeConsumableStacks()
    {
        int maxStack = 3;

        for (int i = 0; i < gearInstanceInventory.Count; i++)
        {
            var a = gearInstanceInventory[i] as ConsumableInstance;
            if (a == null) continue;

            for (int j = i + 1; j < gearInstanceInventory.Count; j++)
            {
                var b = gearInstanceInventory[j] as ConsumableInstance;
                if (b == null) continue;

                if (a.gearSO != b.gearSO) continue;

                int space = maxStack - a.quantityAvailable;
                if (space <= 0) break;

                int transfer = Mathf.Min(space, b.quantityAvailable);

                a.quantityAvailable += transfer;
                b.quantityAvailable -= transfer;

                if (b.quantityAvailable <= 0)
                    gearInstanceInventory[j] = null;
            }
        }
    }

}