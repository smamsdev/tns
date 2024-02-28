using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleTrigger : ToTrigger
{
    [SerializeField] Battle battleToTrigger;


    public override IEnumerator DoAction()
    {
        CombatManagerV3 combatManager = GameObject.Find("CombatManager").GetComponent<CombatManagerV3>();

        combatManager.battleScheme = battleToTrigger;

        combatManager.SetBattleSetupBattle();

        yield return null;
    }
}

