using System.Collections;
using UnityEngine;

public class ChargingStationToTrigger : ToTrigger
{
    [SerializeField] ChargerSO chargerSO;
    public InventorySO playerinv;
    public ChargingMenuManager chargingMenuManager;

    private void OnEnable()
    {
        if (chargerSO.chargingSlots.Length > 5)
            Debug.LogError(this.gameObject.name + "Charger exceeds limit of 5");

        if (chargerSO.costPerCharge == 0)
            Debug.LogError(this.gameObject.name + "fee set to 0");

        chargingMenuManager.gameObject.SetActive(false);
    }

    public override IEnumerator TriggerFunction()
    {
        chargingMenuManager.gameObject.SetActive(true);
        chargingMenuManager.chargingMainMenu.chargerSO = chargerSO;
        chargingMenuManager.OpenChargingStation();
        yield return null;
    }
}
