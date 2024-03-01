using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{

    public string gearID;

    private void Awake()
    {
        gearID = this.name;
    }

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
