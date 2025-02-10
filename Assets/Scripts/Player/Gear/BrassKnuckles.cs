using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BrassKnuckles : Gear
{

  //  [SerializeField] combatManager.playerMoveManager moveManager;


    public override void ApplyAttackGear()


    {
      //  if (moveManager.firstMoveIs == 1 || moveManager.secondMoveIs == 1)
      //  {
      //      GameObject.Find("Player").GetComponent<EquippedGear>().playerPermanentStats.attackPowerGearMod += 11;
      //  }
    }

    public override void ResetAttackGear()

    {
       // GameObject.Find("Player").GetComponent<EquippedGear>().playerPermanentStats.attackPowerGearMod = 0;
    }

}
