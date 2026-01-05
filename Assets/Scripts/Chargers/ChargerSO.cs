using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ChargerSO : ScriptableObject
{
    public Equipment[] chargingSlots = new Equipment[3];
}
