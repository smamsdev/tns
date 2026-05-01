using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public abstract class GearSO : ScriptableObject
{
    public string GearName { get => gearName; }
    [SerializeField] string gearName;

    public string GearDescription { get => gearDescription; }
    [TextArea(2, 5)][SerializeField] string gearDescription;

    public int Value { get => value; }
    [SerializeField] int value;

    public GameObject GearPrefab {get => gearPrefab;}
    public GameObject gearPrefab;
}
