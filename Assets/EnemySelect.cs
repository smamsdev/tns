using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelect : State
{
    public SelectEnemyMenuScript selectEnemyMenuScript;
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        CombatEvents.SendMove += SelectEnemyTarget;

        combatManager.combatUIScript.secondMoveMenu.SetActive(false);
        combatManager.combatUIScript.ShowEnemySelectMenu(true);
        yield break;
    }

    void SelectEnemyTarget(int moveValue)

    {
        combatManager.selectedEnemy = moveValue;
        combatManager.SetState(combatManager.attackTarget);
        CombatEvents.SendMove -= SelectEnemyTarget;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManager.SetState(combatManager.secondMove);
            CombatEvents.InputCoolDown?.Invoke(0.2f);
        }
    }

    private void OnDisable()
    {
        CombatEvents.SendMove -= SelectEnemyTarget;
    }

}
