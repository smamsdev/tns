using System.Collections;
using UnityEngine;

public class ChargingStation : ToTrigger
{
    [SerializeField] ChargerSO charger;
    public InventorySO playerinv;

    public void LoadToChargeSlot(EquipmentInstance equipmentInstance, int slot)
    {
        equipmentInstance.StartCharging();
    }

    public override IEnumerator TriggerFunction()
    {
        Debug.Log("test");
        yield return null;
    }
}
