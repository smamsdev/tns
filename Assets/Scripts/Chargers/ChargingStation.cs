using System.Collections;
using UnityEngine;

public class ChargingStation : ToTrigger
{
    [SerializeField] ChargerSO charger;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            charger.chargingSlots[0].StartCharging();

        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            charger.chargingSlots[0].UpdateCharge();

        }
    }

    public void LoadToChargeSlot(EquipmentSO equipment, int slot)
    {
        equipment.StartCharging();
    }

    public override IEnumerator TriggerFunction()
    {
        Debug.Log("test");
        yield return null;
    }
}
