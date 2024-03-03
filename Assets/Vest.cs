using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vest : Gear
{


    [SerializeField] PlayerMoveManagerSO moveManager;

    [TextArea(2, 5)] public string description;

    private void Awake()
    {
        gearID = this.name; 
    }


    public override void ApplyFendGear()

    {
        if (moveManager.firstMoveIs == 2 || moveManager.secondMoveIs == 2)
        {
            GameObject.Find("Player").GetComponent<GearEquip>().playerStats.fendPowerGearMod += 5;
        }
    }

    public override void ResetFendGear()

    {
        GameObject.Find("Player").GetComponent<GearEquip>().playerStats.attackPowerGearMod = 0;
    }

}
