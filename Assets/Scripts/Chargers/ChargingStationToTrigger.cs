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
            Debug.LogError("Charger exceeds limit of 5");
    }

    public override IEnumerator TriggerFunction()
    {
        chargingMenuManager.gameObject.SetActive(true);
        chargingMenuManager.chargingMainMenu.chargerSO = chargerSO;
        chargingMenuManager.OpenChargingStation();
        yield return null;
    }
}
