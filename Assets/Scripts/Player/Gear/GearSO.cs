using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public abstract class GearSO : ScriptableObject
{   //should i use getters on these to be safe?
    public string gearName;
    [TextArea(2, 5)] public string gearDescription;
    public bool isCurrentlyEquipped;
    public int value;
    public GameObject gearPrefab;
    [System.NonSerialized] public Gear gearInstance;
}
