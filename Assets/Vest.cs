using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vest : Gear
{
  //  [SerializeField] combatManager.playerMoveManager moveManager;

    private void Awake()
    {
        gearID = this.name; 
    }


    public override void ApplyFendGear()

    {
      //if (moveManager.firstMoveIs == 2 || moveManager.secondMoveIs == 2)
      //{
      //    GameObject.Find("Player").GetComponent<EquippedGear>().playerPermanentStats.fendPowerGearMod += 5;
      //}
    }

    public override void ResetFendGear()

    {
  //      GameObject.Find("Player").GetComponent<EquippedGear>().playerPermanentStats.attackPowerGearMod = 0;
    }

}
