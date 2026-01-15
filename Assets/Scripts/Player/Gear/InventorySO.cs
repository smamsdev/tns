using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]

public class InventorySO : ScriptableObject
{
    [SerializeReference]
    public List<GearInstance> gearInstanceInventory = new List<GearInstance>();
    public List<GearInstance> gearInstanceEquipped = new List<GearInstance>();
    public int equipSlotsAvailable;
}