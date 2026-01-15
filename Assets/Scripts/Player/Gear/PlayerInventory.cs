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
        inventorySO.gearInstanceInventory.Add(gearInstanceToAdd);
        //inventorySO.gearInstanceInventory.Sort((a, b) => a.name.CompareTo(b.name));
    }


    //         if (gearSO is EquipmentSO equipment)
    //     {
    //         inventorySO.gearInventory.Add(equipment);
    //     }
    //
    //     else if (gearSO is ConsumbableSO consumable)
    //
    // if (!inventorySO.gearInventory.Contains(consumable))
    //     inventorySO.gearInventory.Add(consumable);
    //
    // consumable.quantityAvailable++;


    public void RemoveGearFromInventory(GearInstance gearInstanceToRemove)
    {
        inventorySO.gearInstanceInventory.Remove(gearInstanceToRemove);
    }

    public void EquipGearToSlot(GearInstance gearInstanceToEquip, int equipSlotNumber)
    {
        if (inventorySO.gearInstanceEquipped[equipSlotNumber].gearSO != null)
        {
            UnequipGearFromSlot(inventorySO.gearInstanceEquipped[equipSlotNumber]);
        }

        gearInstanceToEquip.isCurrentlyEquipped = true;

        if (gearInstanceToEquip.gearSO is ConsumbableSO consumable)
        {
            consumable.quantityAvailable--;
        }

        inventorySO.gearInstanceEquipped[equipSlotNumber] = gearInstanceToEquip;
    }

    public void UnequipGearFromSlot(GearInstance gearSlotToUnequip)
    {
        gearSlotToUnequip.isCurrentlyEquipped= false;
        int i = inventorySO.gearInstanceEquipped.IndexOf(gearSlotToUnequip);
        inventorySO.gearInstanceEquipped[i] = null;
    }

    public void GearConsumed(GearSO gearToUnequip)
    {
        //gearToUnequip.isCurrentlyEquipped = false;
        //int index = inventorySO.equippedGear.IndexOf(gearToUnequip);
        //inventorySO.equippedGear[index] = null;

        Debug.Log("fix");
    }
}