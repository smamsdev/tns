using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelectState : State
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

    public void CombatantSelected(EnemySelectButtonScript enemySelectScript)
    {
        combatManager.playerCombat.targetCombatant = enemySelectScript.combatant;
        enemySelectMenuUI.DeselectEnemy(enemySelectScript);
        enemySelectMenuUI.isEnemySlotsInitialized = false;
        combatManager.SetState(combatManager.applyMove);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManager.cameraFollow.transformToFollow = combatManager.playerCombat.transform;
            enemySelectMenuUI.DeselectEnemy(enemySelectMenuUI.enemySelectButtonScriptHighlighted);
            enemySelectMenuUI.DisplayMenu(false);

            combatManager.playerCombat.CombatLookDirX = previousLookDirX;

            combatManager.SetState(combatManager.styleSelectState);
        }
    }
}
