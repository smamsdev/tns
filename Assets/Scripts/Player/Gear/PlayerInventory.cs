using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventorySO inventorySO;

    public EquipmentSO testGear;

    public void InstantiateAllEquippedGear(CombatManager combatManager)
    {
        foreach (GearInstance gearInstance in inventorySO.gearInstanceEquipped)
        {
            Debug.Log("fix");

          //  if (gearInstance != null)
          //  { 
          //      GameObject gearInstanceGO = Instantiate(gearInstance.gearPrefab);
          //      gearInstanceGO.name = gearInstance.gearName + "Instance";
          //      gearInstanceGO.transform.SetParent(this.transform, false);
          //
          //      Gear gearInstance = gearInstanceGO.GetComponent<Gear>();
          //      gearSO.gearInstance = gearInstance;
          //      gearInstance.combatManager = combatManager;
          //      gearInstance.OnEquipGear();
          //
          //      if (gearSO is ConsumbableSO)
          //      { gearInstance.turnsUntilConsumed = -1; }
          //  }
        }
    }

    public void InstantiateNewEquippedGear(CombatManager combatManager, GearSO newGearSO)
    {
        GameObject gearInstanceGO = Instantiate(newGearSO.gearPrefab);
        gearInstanceGO.name = newGearSO.gearName + "Instance";
        gearInstanceGO.transform.SetParent(this.transform, false);

        Gear gearInstance = gearInstanceGO.GetComponent<Gear>();
        newGearSO.gearInstance = gearInstance;
        gearInstance.combatManager = combatManager;

        if (newGearSO is EquipmentSO)
        { gearInstance.turnsUntilConsumed = -1; }
    }

    public void DestroyGearInstance(GearSO gearSO)
    {
        Destroy(gearSO.gearInstance.gameObject);
        gearSO.gearInstance = null;
    }

    public void AddGearToInventory(GearInstance gearInstanceToAdd)
    {
        if (gearInstanceToAdd is EquipmentInstance equipmentInstance)
        {
            inventorySO.gearInstanceInventory.Add(equipmentInstance);
        }

        else if (gearInstanceToAdd is ConsumableInstance consumableInstance)
        {
            foreach (GearInstance gearInstanceToCheck in inventorySO.gearInstanceInventory)
            {
                if (gearInstanceToCheck.gearSO == consumableInstance.gearSO)
                {
                    ConsumableInstance existingConsumableInstance = gearInstanceToCheck as ConsumableInstance;
                    existingConsumableInstance.quantityAvailable++;
                    return;
                }
            }

            consumableInstance.quantityAvailable = 1;
            inventorySO.gearInstanceInventory.Add(consumableInstance);
        }

        else
            Debug.Log("something went wrong");

        inventorySO.gearInstanceInventory.Sort((a, b) => a.gearSO.gearName.CompareTo(b.gearSO.gearName));
    }

    public void RemoveGearFromInventory(GearInstance gearInstanceToRemove)
    {
        if (gearInstanceToRemove is EquipmentInstance equipmentInstance)
        {
            inventorySO.gearInstanceInventory.Remove(equipmentInstance);
        }

        else if (gearInstanceToRemove is ConsumableInstance consumableInstance)
        {
            consumableInstance.quantityAvailable--;

            if (consumableInstance.quantityAvailable == 0)
                inventorySO.gearInstanceInventory.Remove(consumableInstance);
        }

        else
            Debug.Log("something went wrong");
    }

    public void EquipGearToSlot(GearInstance gearInstanceToEquip, int equipSlotNumber)
    {
        if (inventorySO.gearInstanceEquipped[equipSlotNumber].gearSO != null)
        {
            UnequipGearFromSlot(inventorySO.gearInstanceEquipped[equipSlotNumber]);
        }

        gearInstanceToEquip.isCurrentlyEquipped = true;

        if (gearInstanceToEquip is ConsumableInstance consumableInstance)
        {
            consumableInstance.quantityAvailable--;
        }

        inventorySO.gearInstanceEquipped[equipSlotNumber] = gearInstanceToEquip;
    }

    public void UnequipGearFromSlot(GearInstance gearInstnaceToUnequip)
    {
        gearInstnaceToUnequip.isCurrentlyEquipped = false;
        int i = inventorySO.gearInstanceEquipped.IndexOf(gearInstnaceToUnequip);
        inventorySO.gearInstanceEquipped[i] = null;

        if (gearInstnaceToUnequip is ConsumableInstance consumableInstance) 
            consumableInstance.quantityAvailable++;
    }

    public void GearConsumed(GearSO gearToUnequip)
    {
        //gearToUnequip.isCurrentlyEquipped = false;
        //int index = inventorySO.equippedGear.IndexOf(gearToUnequip);
        //inventorySO.equippedGear[index] = null;

        Debug.Log("fix");
    }
}