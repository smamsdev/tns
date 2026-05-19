using UnityEngine;

[CreateAssetMenu]

public class ConsumableSO : GearSO
{
    public override GearInstance CreateInstance()
    {
        var newConsumableInstance = new ConsumableInstance(this);
        newConsumableInstance.gearSO = this;
        newConsumableInstance.quantityAvailable = 1;

        return newConsumableInstance;
    }
}
