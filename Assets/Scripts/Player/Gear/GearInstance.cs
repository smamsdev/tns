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
    [SerializeField] float charge;
    public int Charge
    {
        get{return Mathf.RoundToInt(charge);}

        set{charge = Charge; }
    }

    [SerializeField] float chargesAccrued;
    public int ChargesAccrued
    {
        get { return Mathf.RoundToInt(chargesAccrued); }

        set { chargesAccrued = ChargesAccrued;}
    }

    public int lastAppliedCharge;



    public float startChargeTimeStamp;

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
        chargesAccrued = 0;
    }

    // apply the amount of charge based on the last playtime the function was called
    public void UpdateCharge()
    {
        float elapsed = Time.time - startChargeTimeStamp;

        charge += elapsed;
        chargesAccrued += elapsed;

        charge = Mathf.Min(charge, ((EquipmentSO)gearSO).maxPotential);

        startChargeTimeStamp = Time.time;
    }

    public float ChargePercentage()
    {
        float chargePer = charge / ((EquipmentSO)gearSO).maxPotential * 100;
        chargePer = Mathf.RoundToInt(chargePer);
        return chargePer;
    }
}
