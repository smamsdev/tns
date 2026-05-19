using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public abstract class GearSO : ScriptableObject
{
    public string GearName { get => gearName; }
    [SerializeField] string gearName;

    public StatModifier[] StatModifiers{ get => statModifiers; }
    [SerializeField] StatModifier[] statModifiers;

    public string GearDescription { get => gearDescription; }
    [TextArea(2, 5)][SerializeField] string gearDescription;

    public int Value { get => value; }
    [SerializeField] int value;

    public GameObject MonobehaviourPrefab { get => monobehaviourPrefab; }
    [SerializeField] GameObject monobehaviourPrefab;

    public abstract GearInstance CreateInstance();

    private void OnValidate()
    {
        if (MonobehaviourPrefab == null)
        {
            Debug.Log("prefab is null on " + GearName, this);
            Debug.DebugBreak();
        }
    }
}
