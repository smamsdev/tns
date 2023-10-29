using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMove : State
{
    public FirstMove(CombatManagerV3 _combatManagerV3) : base(_combatManagerV3)
    {
    }

    public override IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        combatManagerV3.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

        combatManagerV3.combatUIScript.ShowFirstMoveMenu();
        combatManagerV3.playerMoveManager.firstMoveIs = 0;

        CombatEvents.ShowHideFendDisplay?.Invoke(false);
        CombatEvents.GetEnemyAttackPower?.Invoke();

        yield break;
    }

    public override void Update()
    {

    }

}
