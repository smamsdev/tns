using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GearMonoBehaviour : MonoBehaviour
{
    [HideInInspector] public CombatManager combatManager;
    [SerializeField] GearInstance gearInstance;

    public void SetGearInstance(GearInstance gearInstance)
    {
        if (gearInstance is EquipmentInstance equipmentInstance)
            gearInstance = equipmentInstance;

        else gearInstance = gearInstance as ConsumableInstance;
    }

    public virtual IEnumerator ApplyGear()
    {
        yield return null;
    }

    public virtual void OnEquipGear()
    {
        return;
    }
}
