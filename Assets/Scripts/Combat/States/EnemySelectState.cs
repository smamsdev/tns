using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelectState : State
{
    public SelectEnemyMenuScript selectEnemyMenuScript;
    int previousLookDirX;

    public override IEnumerator StartState()
    {
        previousLookDirX = combatManager.playerCombat.CombatLookDirX;
        selectEnemyMenuScript.InitializeButtonSlots();
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.enemySelectMenu, true);
        combatManager.combatMenuManager.selectEnemyMenuDefaultButton.Select();
        yield break;
    }

    public void CombatantSelected(EnemySelectButtonScript enemySelectScript)
    {
        combatManager.playerCombat.targetToAttack = enemySelectScript.combatant;
        selectEnemyMenuScript.DeselectEnemy(enemySelectScript);
        selectEnemyMenuScript.isEnemySlotsInitialized = false;
        combatManager.SetState(combatManager.applyMove);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManager.cameraFollow.transformToFollow = combatManager.playerCombat.transform;
            selectEnemyMenuScript.DeselectEnemy(selectEnemyMenuScript.enemySelectButtonScriptHighlighted);
            combatManager.combatMenuManager.SetButtonNormalColor(combatManager.secondMove.lastButtonSelected, Color.white);
            combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.enemySelectMenu, false);

            combatManager.playerCombat.CombatLookDirX = previousLookDirX;

            combatManager.SetState(combatManager.secondMove);
        }
    }
}
