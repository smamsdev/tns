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
    public int inventorySlotsAvailable;

    [Header("For Actors")]
    [SerializeReference]
    public List<GearInstance> gearInstanceEquipped = new List<GearInstance>();

    void AddEquipSlot()
    {
        gearInstanceEquipped.Add(new GearInstance());
    }

    public bool AttemptAddGearToInventory(GearInstance gearInstanceToAdd)
    {
        if (gearInstanceToAdd is EquipmentInstance equipmentInstance)
        {
            if (gearInstanceInventory.Count >= inventorySlotsAvailable)
                return false;

            gearInstanceInventory.Add(equipmentInstance);
            SortInventory();
            return true;

        }

        else if (gearInstanceToAdd is ConsumableInstance consumableInstance)
        {
            Debug.Log("consumable detected");

            for (int i = 0; i < gearInstanceInventory.Count; i++)
            {

                Debug.Log("looping" + gearInstanceInventory[i].gearSO);
                // check if this consumable exists in inventory
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


            if (gearInstanceInventory.Count >= inventorySlotsAvailable)
            {
                Debug.Log("no slots available");
                return false;
            }


            Debug.Log("fresh instance created");
            consumableInstance.quantityAvailable = 1;
            gearInstanceInventory.Add(consumableInstance);
            SortInventory();
            return true;
        }

        else
        {
            Debug.Log("something went wrong");
            return false;
        }






    }

    public void SortInventory()
    {
        gearInstanceInventory.Sort((a, b) => a.gearSO.gearName.CompareTo(b.gearSO.gearName));
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

    public void RemoveGearFromInventory(GearInstance gearInstanceToRemove)
    {
        if (gearInstanceToRemove is EquipmentInstance equipmentInstance)
        {
            gearInstanceInventory.Remove(equipmentInstance);
        }

        else if (gearInstanceToRemove is ConsumableInstance consumableInstance)
        {
            consumableInstance.quantityAvailable--;

            if (consumableInstance.quantityAvailable <= 0)
                gearInstanceInventory.Remove(consumableInstance);
        }

        else
            Debug.Log("something went wrong");
    }

}