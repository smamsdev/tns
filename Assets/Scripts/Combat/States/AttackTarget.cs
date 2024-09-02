using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);

        combatManager.CombatUIManager.ShowBodyPartTargetMenu(true);
        combatManager.CombatUIManager.ShowEnemySelectMenu(false);

        yield break;
    }

    public override void CombatOptionSelected(int moveValue)

    {
        combatManager.enemy[combatManager.selectedEnemy].SetEnemyBodyPartTarget(moveValue);
        combatManager.SetState(combatManager.applyMove);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManager.SetState(combatManager.secondMove);
        }
    }
}

