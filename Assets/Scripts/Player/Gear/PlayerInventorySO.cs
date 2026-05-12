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
}