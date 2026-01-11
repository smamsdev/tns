using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]

public class InventorySO : ScriptableObject
{
    public List<GearSO> gearInventory = new List<GearSO>();
    public List<GearSO> equippedGear = new List<GearSO>();
    public int equipSlotsAvailable;

    public List<GearInstance> gearInstances = new List<GearInstance>();
}