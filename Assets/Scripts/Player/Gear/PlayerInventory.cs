using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventorySO inventorySO;

    private void OnEnable()
    {
        foreach (GearSO gear in inventorySO.gearInventory)
        {
            if (gear != null) gear.isCurrentlyEquipped = false;
        }

        foreach (GearSO gear in inventorySO.equippedGear)
        {
            if (gear != null)
            {
                gear.isCurrentlyEquipped = true;
            }
        }
    }

    public void InstantiateAllEquippedGear(CombatManager combatManager)
    {
        foreach (GearSO gearSO in inventorySO.equippedGear)
        {
            if (gearSO != null)
            { 
                GameObject gearInstanceGO = Instantiate(gearSO.gearPrefab);
                gearInstanceGO.name = gearSO.gearName + "Instance";
                gearInstanceGO.transform.SetParent(this.transform, false);

                Gear gearInstance = gearInstanceGO.GetComponent<Gear>();
                gearSO.gearInstance = gearInstance;
                gearInstance.combatManager = combatManager;
                gearInstance.OnEquipGear();

                if (gearSO is ConsumbableSO)
                { gearInstance.turnsUntilConsumed = -1; }
            }
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

    public void AddGearToInventory(GearSO gearSO)
    {
        if (gearSO is EquipmentSO equipment)
        {
            inventorySO.gearInventory.Add(equipment);
        }

        else if (gearSO is ConsumbableSO consumable)
        {
            if (!inventorySO.gearInventory.Contains(consumable))
                inventorySO.gearInventory.Add(consumable);

            consumable.quantityAvailable++;
        }

        inventorySO.gearInventory.Sort((a, b) => a.name.CompareTo(b.name));
    }

    public void RemoveGearFromInventory(GearSO gearSO)
    {
        if (gearSO is ConsumbableSO consumable)
        {
            consumable.quantityAvailable--;
            if (consumable.quantityAvailable <= 0 )
                inventorySO.gearInventory.Remove(consumable);   
        }

        else if (gearSO is EquipmentSO equipment)
            inventorySO.gearInventory.Remove(equipment);
    }

    public void EquipGearToSlot(GearSO gearSlOToEquip, int equipSlotNumber)
    {
        if (inventorySO.equippedGear[equipSlotNumber] != null)
        {
            UnequipGearFromSlot(inventorySO.equippedGear[equipSlotNumber]);
        }

        gearSlOToEquip.isCurrentlyEquipped = true;

        if (gearSlOToEquip is ConsumbableSO consumable)
        {
            consumable.quantityAvailable--;
        }

        inventorySO.equippedGear[equipSlotNumber] = gearSlOToEquip;
    }

    public void UnequipGearFromSlot(GearSO gearToUnequip)
    {
        gearToUnequip.isCurrentlyEquipped = false;
        int index = inventorySO.equippedGear.IndexOf(gearToUnequip);
        inventorySO.equippedGear[index] = null;

        if (gearToUnequip is ConsumbableSO consumable)
        {
            consumable.quantityAvailable++;
        }
    }

    public void GearConsumed(GearSO gearToUnequip)
    {
        gearToUnequip.isCurrentlyEquipped = false;
        int index = inventorySO.equippedGear.IndexOf(gearToUnequip);
        inventorySO.equippedGear[index] = null;
    }
}