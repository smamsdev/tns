using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class GearSO : ScriptableObject
{
    public string gearID;
    public string gearName;
    [TextArea(2, 5)] public string gearDescription;
    public bool isCurrentlyEquipped;
    public int quantityInInventory;
    public bool isConsumable;
    public int value;
    public GameObject gearPrefab;
    [System.NonSerialized] public Gear gearInstance;
}
