using System.Collections;
using UnityEngine;

public class ChargingStation : ToTrigger
{
    [SerializeField] ChargerSO chargerSO;
    public InventorySO playerinv;
    public ChargingMenuManager chargingMenuManager;

    public void LoadToChargeSlot(EquipmentInstance equipmentInstance, int slot)
    {
        equipmentInstance.StartCharging();
    }

    public override IEnumerator TriggerFunction()
    {
        chargingMenuManager.gameObject.SetActive(true);
        chargingMenuManager.chargingMainMenu.chargerSO = chargerSO;
        chargingMenuManager.OpenChargingStation();
        yield return null;
    }
}
