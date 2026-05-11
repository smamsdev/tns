using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncloseSelectState : State
{
    public EnemySelectMenuUI enemySelectMenuUI;
    int previousLookDirX;

    public override IEnumerator StartState()
    {
        previousLookDirX = combatManager.playerCombat.CombatLookDirX;
        enemySelectMenuUI.InitializeButtonSlots();
        enemySelectMenuUI.DisplayMenu(true);
        enemySelectMenuUI.menuButtons[enemySelectMenuUI.highlightedButtonIndex].Select();
        yield break;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManager.cameraFollow.transformToFollow = combatManager.playerCombat.transform;
            combatManager.tacticalSelectState.isEnclosing = false;
            enemySelectMenuUI.DeselectEnemy(enemySelectMenuUI.enemySelectButtonScriptHighlighted);
            combatManager.playerCombat.CombatLookDirX = previousLookDirX;
            combatManager.SetState(combatManager.tacticalSelectState);
        }
    }
}
