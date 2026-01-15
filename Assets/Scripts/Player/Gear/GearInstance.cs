using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static Unity.VisualScripting.Member;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class GearInstance
{
    public GearSO gearSO;
    public bool isCurrentlyEquipped = false;
}

[System.Serializable]
public class ConsumableInstance : GearInstance
{
    public int quantityAvailable;

    public ConsumableInstance(ConsumableInstance sourceToClone)
    {
        gearSO = sourceToClone.gearSO;
        isCurrentlyEquipped = sourceToClone.isCurrentlyEquipped;
        quantityAvailable = sourceToClone.quantityAvailable;
    }

    public ConsumableInstance()
    {
        gearSO = null;
        isCurrentlyEquipped = false;
        quantityAvailable = 0;
    }
}

[System.Serializable]
public class EquipmentInstance : GearInstance
{
    public int charge;
    int maxCharge;

    float startChargeTimeStamp;

    public EquipmentInstance(EquipmentInstance sourceToClone)
    {
        gearSO = sourceToClone.gearSO;
        isCurrentlyEquipped = sourceToClone.isCurrentlyEquipped;
        charge = sourceToClone.charge;
    }

    public EquipmentInstance()
    {
        gearSO = null;
        isCurrentlyEquipped = false;
        charge = 0;
    }

    public void StartCharging()
    {
        startChargeTimeStamp = Time.time;
        maxCharge = ((EquipmentSO)gearSO).maxPotential;
    }

    public void UpdateCharge()
    {
        if (startChargeTimeStamp == 0)
        {
            Debug.Log(this + " has not been loaded to charge");
            return;
        }

        float elapsed = Time.time - startChargeTimeStamp;
        int chargeChange = Mathf.RoundToInt(elapsed);
        chargeChange = Mathf.Clamp(chargeChange, 0, maxCharge - charge);
        charge += chargeChange;

        startChargeTimeStamp = Time.time; 
    }
}
