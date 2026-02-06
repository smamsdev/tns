using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ChargerSO : ScriptableObject
{
    public EquipmentInstance[] chargingSlots = new EquipmentInstance[3];
    public int costPerCharge;

    public void ApplyChargeToSlot(int i)
    {
        if (chargingSlots[i] == null || chargingSlots[i].gearSO == null)
            return;

            chargingSlots[i].UpdateCharge();
    }
}
