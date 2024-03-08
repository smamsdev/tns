using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleTrigger : ToTrigger
{
    [SerializeField] Battle battleToTrigger;


    public override IEnumerator DoAction()
    {
        CombatManager combatManager = GameObject.Find("CombatManager").GetComponent<CombatManager>();

        combatManager.battleScheme = battleToTrigger;

        combatManager.StartBattle();

        yield return null;
    }
}

