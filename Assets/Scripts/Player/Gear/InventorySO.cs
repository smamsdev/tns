using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]

public class InventorySO : ScriptableObject
{
    [Header("Debug")]
    public GearSO debugGearToAddAsInstance;

    [SerializeReference]
    public List<GearInstance> gearInstanceInventory = new List<GearInstance>();
    public int inventorySlotsAvailable;


    [SerializeReference]
    public List<GearInstance> gearInstanceEquipped = new List<GearInstance>();

}