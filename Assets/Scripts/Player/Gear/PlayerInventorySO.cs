using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]

public class PlayerInventorySO : InventorySO
{
    [SerializeReference]
    public List<GearInstance> gearInstanceEquipped = new List<GearInstance>();
    public List<TrenchStructureSO> trenchStructuresInventory = new List<TrenchStructureSO>();

    [Header("Debug")]
    public int debugInventorySlotToEquip;
    public int debugEquipSlotToAddTo;

    void AddEquipSlot()
    {
        gearInstanceEquipped.Add(new GearInstance());
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

   // public void InstantiateAllEquippedGear(CombatManager combatManager)
   // {
   //     foreach (GearInstance gearInstance in inventorySO.gearInstanceEquipped)
   //     {
   //         Debug.Log("fix");
   //
   //         //  if (gearInstance != null)
   //         //  { 
   //         //      GameObject gearInstanceGO = Instantiate(gearInstance.gearPrefab);
   //         //      gearInstanceGO.name = gearInstance.gearName + "Instance";
   //         //      gearInstanceGO.transform.SetParent(this.transform, false);
   //         //
   //         //      Gear gearInstance = gearInstanceGO.GetComponent<Gear>();
   //         //      gearSO.gearInstance = gearInstance;
   //         //      gearInstance.combatManager = combatManager;
   //         //      gearInstance.OnEquipGear();
   //         //
   //         //      if (gearSO is ConsumbableSO)
   //         //      { gearInstance.turnsUntilConsumed = -1; }
   //         //  }
   //     }
   // }

   // public void InstantiateNewEquippedGear(CombatManager combatManager, GearSO newGearSO)
   // {
   //     GameObject gearInstanceGO = Instantiate(newGearSO.gearPrefab);
   //     gearInstanceGO.name = newGearSO.gearName + "Instance";
   //     gearInstanceGO.transform.SetParent(this.transform, false);
   //
   //     Gear gearInstance = gearInstanceGO.GetComponent<Gear>();
   //     newGearSO.gearInstance = gearInstance;
   //     gearInstance.combatManager = combatManager;
   //
   //     if (newGearSO is EquipmentSO)
   //     { gearInstance.turnsUntilConsumed = -1; }
   // }
   //
   // public void DestroyGearInstance(GearSO gearSO)
   // {
   //     Destroy(gearSO.gearInstance.gameObject);
   //     gearSO.gearInstance = null;
   // }


}