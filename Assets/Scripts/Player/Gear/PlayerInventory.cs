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
            gear.isCurrentlyEquipped = false;
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
                gearInstanceGO.name = gearSO.gearID + "Instance";
                gearInstanceGO.transform.SetParent(this.transform, false);

                Gear gearInstance = gearInstanceGO.GetComponent<Gear>();
                gearSO.gearInstance = gearInstance;
                gearInstance.combatManager = combatManager;
                gearInstance.OnEquipGear();

                if (!gearSO.isConsumable)
                { gearInstance.turnsUntilConsumed = -1; }
            }
        }
    }

    public void InstantiateNewEquippedGear(CombatManager combatManager, GearSO newGearSO)
    {
        GameObject gearInstanceGO = Instantiate(newGearSO.gearPrefab);
        gearInstanceGO.name = newGearSO.gearID + "Instance";
        gearInstanceGO.transform.SetParent(this.transform, false);

        Gear gearInstance = gearInstanceGO.GetComponent<Gear>();
        newGearSO.gearInstance = gearInstance;
        gearInstance.combatManager = combatManager;

        if (!newGearSO.isConsumable)
        { gearInstance.turnsUntilConsumed = -1; }
    }

    public void DestroyGearInstance(GearSO gearSO)
    {
        Destroy(gearSO.gearInstance.gameObject);
        gearSO.gearInstance = null;
    }

    public void AddGearToInventory(GearSO gear)
    {
        inventorySO.gearInventory.Add(gear);
        gear.quantityInInventory++;
    }

    public void RemoveGearFromInventory(GearSO gear)
    {
        inventorySO.gearInventory.Remove(gear);
        gear.quantityInInventory++;
    }

    public void EquipGearToSlot(GearSO gearSOToEquip, int equipSlotNumber)
    {
        if (inventorySO.equippedGear[equipSlotNumber] != null)
        {
            UnequipGearFromSlot(inventorySO.equippedGear[equipSlotNumber]);
        }

        gearSOToEquip.isCurrentlyEquipped = true;
        gearSOToEquip.quantityInInventory--;
        inventorySO.equippedGear[equipSlotNumber] = gearSOToEquip;
    }
        
    public void UnequipGearFromSlot(GearSO gearToUnequip)
    {
        gearToUnequip.isCurrentlyEquipped = false;
        gearToUnequip.quantityInInventory++;
        int index = inventorySO.equippedGear.IndexOf(gearToUnequip);
        inventorySO.equippedGear[index] = null;
    }

    public void GearConsumed(GearSO gearToUnequip)
    {
        gearToUnequip.isCurrentlyEquipped = false;
        int index = inventorySO.equippedGear.IndexOf(gearToUnequip);
        inventorySO.equippedGear[index] = null;
    }
}