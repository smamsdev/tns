using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gear : MonoBehaviour
{

    public string gearID;
    [TextArea(2, 5)] public string gearDescription;
    public bool isEquipment;
    public int quantityInInventory;
    public int value;

    public virtual void ApplyAttackGear()

    {
     
    }

    public virtual void ResetAttackGear()

    { }

    public virtual void ApplyFendGear()

    { }

    public virtual void ResetFendGear()

    { }

}
