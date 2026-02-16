using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventorySO inventorySO;

    //eventually delete this entirely i hope


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

}