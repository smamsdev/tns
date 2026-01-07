using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu]

public class EquipmentSO : GearSO
{
    public int Potential
    {
        get
        {
            return potential;
        }
        set
        {
            potential = Mathf.Clamp(value, 0, maxPotential);
        }

    }

    [SerializeField] int potential;
    [SerializeField] int maxPotential;

    float startChargeTimeStamp;

    public void StartCharging()
    {
        startChargeTimeStamp = Time.time;
    }

    public void UpdateCharge()
    {
        if (startChargeTimeStamp == 0)

        { 
            Debug.Log(this.name + " has not been loaded to charge");
            return;
        }

        int charge = Mathf.RoundToInt(startChargeTimeStamp + Time.time);
        Potential += charge; 
    }
}
