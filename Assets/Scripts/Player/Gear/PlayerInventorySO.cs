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
        if (gearInstanceToEquip is EquipmentInstance equipmentInstance)
            EquipEquipmentToSlot(equipmentInstance, equipSlotNumber);

        else if (gearInstanceToEquip is ConsumableInstance consumableInstance)
            EquipConsumableToSlot(consumableInstance, equipSlotNumber);


    }

    void EquipEquipmentToSlot(EquipmentInstance equipmentInstanceToEquip, int equipSlotNumber)
    {
        equipmentInstanceToEquip.isCurrentlyEquipped = true;

        if (gearInstanceEquipped[equipSlotNumber].gearSO != null)
            UnequipGear(gearInstanceEquipped[equipSlotNumber]);

        gearInstanceEquipped[equipSlotNumber] = equipmentInstanceToEquip;
    }

    void EquipConsumableToSlot(ConsumableInstance consumableInstanceToEquip, int equipSlotNumber)
    {
        ConsumableInstance newConsumableIntance = new ConsumableInstance(consumableInstanceToEquip);
        
        newConsumableIntance.isCurrentlyEquipped = true;
        RemoveGearFromInventory(consumableInstanceToEquip, true);

        if (gearInstanceEquipped[equipSlotNumber].gearSO != null)
            UnequipGear(gearInstanceEquipped[equipSlotNumber]);

        gearInstanceEquipped[equipSlotNumber] = newConsumableIntance;
    }

    public void UnequipGear(GearInstance gearInstanceToUnequip)
    {
        int i = gearInstanceToUnequip.EquippedSlotInt(this);

        gearInstanceToUnequip.isCurrentlyEquipped = false;
        gearInstanceEquipped[i] = new GearInstance();
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