using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu]

public class EquipmentSO : GearSO
{
    public int MaxCharge
    {
        get => maxCharge;
        set => maxCharge = Mathf.Clamp(value, 0, 9999);
    }
    [SerializeField] int maxCharge;

    public override GearInstance CreateInstance()
    {
        var newEquipmentInstance = new EquipmentInstance(this);
        newEquipmentInstance.gearSO = this;

        return newEquipmentInstance;
    }
}
