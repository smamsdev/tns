using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelect : State
{
    public SelectEnemyMenuScript selectEnemyMenuScript;
    Vector2 previousLookDir;

    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.selectEnemyMenuScript.InitializeButtonSlots();
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.enemySelectMenu, true);
        combatManager.combatMenuManager.thirdMenuFirstButton = selectEnemyMenuScript.lastEnemySelected;
        combatManager.combatMenuManager.thirdMenuFirstButton.Select();
        previousLookDir = combatManager.playerCombat.movementScript.lookDirection;

        yield break;
    }

    public override void CombatOptionSelected(int moveValue)
    {
        combatManager.playerCombat.targetToAttack = combatManager.enemies[moveValue];
        combatManager.combatMenuManager.thirdMenuFirstButton = buttonSelected;
        selectEnemyMenuScript.DeselectEnemy(selectEnemyMenuScript.enemyhighlighted);
        combatManager.SetState(combatManager.applyMove);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManager.cameraFollow.transformToFollow = combatManager.player.transform;
            combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.enemySelectMenu, false);
            combatManager.combatMenuManager.SetButtonNormalColor(combatManager.secondMove.buttonSelected, Color.white);
            selectEnemyMenuScript.DeselectEnemy(selectEnemyMenuScript.enemyhighlighted);
            combatManager.playerCombat.movementScript.lookDirection = previousLookDir;

            combatManager.SetState(combatManager.secondMove);

        }
    }
}
