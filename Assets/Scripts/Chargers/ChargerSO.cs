using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ChargerSO : ScriptableObject
{
    public EquipmentInstance[] chargingSlots = new EquipmentInstance[3];
}
