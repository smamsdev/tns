using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BrassKnuckles : Gear
{

    [SerializeField] PlayerMoveManagerSO moveManager;

    [TextArea(2, 5)] public string description;

    private void Awake()
    {
        gearID = this.name;
    }

    public override void ApplyAttackGear()


    {
        if (moveManager.firstMoveIs == 1 || moveManager.secondMoveIs == 1)
        {
            GameObject.Find("Player").GetComponent<GearEquip>().playerStats.attackPowerGearMod += 11;
        }
    }

    public override void ResetAttackGear()

    {
        GameObject.Find("Player").GetComponent<GearEquip>().playerStats.attackPowerGearMod = 0;
    }

}
