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
    public int maxPotential;
}
