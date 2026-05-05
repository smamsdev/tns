using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static Unity.VisualScripting.Member;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class GearInstance
{
    public GearSO gearSO;
    public bool isCurrentlyEquipped = false;

    public string QuantityString()
    {
        if (this is EquipmentInstance equipmentInstance)
            return equipmentInstance.ChargePercentage() + "%";

        if (this is ConsumableInstance consumableInstance)
            return "x " + consumableInstance.quantityAvailable;

        return "";
    }

    public string DescriptionFormatted()
    {
        return "Description: " + gearSO.GearDescription;
    }

    public int BuyValue(int shopMarkupPer)
    {
        return Mathf.RoundToInt(gearSO.Value * (1f + shopMarkupPer / 100f));
    }

    public string BuyValueFormattedString(int shopMarkupPer)
    {
        return "Buy: " + BuyValue(shopMarkupPer).ToString("N0") + " $MAMS";
    }

    public string SellValueFormattedString()
    {
        return "Sell: " + gearSO.Value.ToString("N0") + " $MAMS";
    }

    public int EquippedSlotInt(PlayerInventorySO playerInventorySO)
    {
         return playerInventorySO.gearInstanceEquipped.IndexOf(this);
    }
}

[System.Serializable]
public class ConsumableInstance : GearInstance
{
    public int quantityAvailable;

    public ConsumableInstance(ConsumableInstance sourceToClone)
    {
        gearSO = sourceToClone.gearSO;
        quantityAvailable = 1;
    }

    public ConsumableInstance(GearSO sourceSO)
    {
        gearSO = sourceSO;
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
        get => charge;
        private set => charge = Mathf.Clamp(value, 0f, MaxCharge());
    }

    private float MaxCharge()
    {
        if (gearSO is EquipmentSO equipment)
            return equipment.maxPotential;

        return 0f;
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
        return "Charge: " + Mathf.RoundToInt(Charge) + " / " + MaxPotential();
    }

    public int MaxPotential()
    {
        return ((EquipmentSO)gearSO).maxPotential;
    }

    public void AddCharge(float amount)
    {
        Charge += amount;
    }

    public void RemoveCharge(float amount)
    {
        Charge -= amount;
    }

    public void AddAccruedCharge(int amount)
    {
        payableChargesAccrued += amount;
    }

    public void SetCharge(float value)
    {
        Charge = Mathf.Max(0f, value);
    }

    public void ResetPayableChargesAccrued()
    {
        PayableChargesAccrued = 0;
    }

    public EquipmentInstance()
    {
        gearSO = null;
        isCurrentlyEquipped = false;
        Charge = 0f;
        payableChargesAccrued = 0;
    }

    public EquipmentInstance(EquipmentInstance source)
    {
        gearSO = source.gearSO;
        isCurrentlyEquipped = source.isCurrentlyEquipped;
        Charge = source.Charge;
    }

    public EquipmentInstance(GearSO sourceSO)
    {
        gearSO = sourceSO;
    }
}
