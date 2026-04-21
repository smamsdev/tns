using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static Unity.VisualScripting.Member;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class GearInstance
{
    public GearSO gearSO;
    public bool isCurrentlyEquipped = false;

    public string GearQuantityRemainingString()
    {
        if (this is EquipmentInstance equipmentInstance)
            return equipmentInstance.ChargePercentage() + "%";

        if (this is ConsumableInstance consumableInstance)
            return "x " + consumableInstance.quantityAvailable;

        return "";
    }
}

[System.Serializable]
public class ConsumableInstance : GearInstance
{
    public int quantityAvailable;

    public ConsumableInstance(ConsumableInstance sourceToClone)
    {
        gearSO = sourceToClone.gearSO;
        isCurrentlyEquipped = sourceToClone.isCurrentlyEquipped;
        quantityAvailable = 1;
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
    [SerializeField] private float charge;
    [SerializeField] private int payableChargesAccrued;

    public float Charge
    {
        get => Mathf.RoundToInt(charge);
        private set => charge = Mathf.Max(0f, value);
    }

    public int PayableChargesAccrued
    {
        get => Mathf.RoundToInt(payableChargesAccrued);
        private set => payableChargesAccrued = value;
    }

    public float ChargePercentage()
    {
        float max = ((EquipmentSO)gearSO).maxPotential;

        if (max <= 0f)
            return 0f;

        return Mathf.RoundToInt((charge / max) * 100f);
    }

    public string ChargeTotalString()
    {
        return "Charge: " + Charge + " / " + MaxPotential();
    }

    public int MaxPotential()
    {
        return ((EquipmentSO)gearSO).maxPotential;
    }

    public void AddCharge(float amount)
    {
        charge += amount;
    }

    public void AddAccruedCharge(int amount)
    {
        payableChargesAccrued += amount;
    }

    public void SetCharge(float value)
    {
        charge = Mathf.Max(0f, value);
    }

    public void ResetPayableChargesAccrued()
    {
        PayableChargesAccrued = 0;
    }

    public EquipmentInstance()
    {
        gearSO = null;
        isCurrentlyEquipped = false;
        charge = 0f;
        payableChargesAccrued = 0;
    }

    public EquipmentInstance(EquipmentInstance source)
    {
        gearSO = source.gearSO;
        isCurrentlyEquipped = source.isCurrentlyEquipped;
        charge = source.charge;
    }
}
