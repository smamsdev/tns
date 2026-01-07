using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ChargerSO : ScriptableObject
{
    public EquipmentSO[] chargingSlots = new EquipmentSO[3];
}
