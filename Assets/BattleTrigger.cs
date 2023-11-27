using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleTrigger : ToTrigger
{
    public override IEnumerator DoAction()
    {
        GetComponent<CombatManagerV3>().SetBattleSetupBattle();
        yield return null;
    }
}

