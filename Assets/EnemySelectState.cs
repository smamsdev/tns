using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelectState : State
{
    public SelectEnemyMenuScript selectEnemyMenuScript;
    Vector2 previousLookDir;

    public override IEnumerator StartState()
    {
        selectEnemyMenuScript.InitializeButtonSlots();
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.enemySelectMenu, true);
        combatManager.combatMenuManager.selectEnemyMenuDefaultButton.Select();
        previousLookDir = combatManager.playerCombat.movementScript.lookDirection;

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

            combatManager.playerCombat.movementScript.lookDirection = previousLookDir;

            combatManager.SetState(combatManager.secondMove);

        }
    }
}
