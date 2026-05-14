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
        enemySelectMenuUI.InitializeButtonSlots(combatManager.enemies);
        enemySelectMenuUI.DisplayMenu(true);
        enemySelectMenuUI.menuButtons[enemySelectMenuUI.highlightedButtonIndex].Select();
        yield break;
    }

    public void CombatantSelected(TargetSelectButtonUI targetSelectButtonUI)
    {
        combatManager.playerCombat.targetCombatant = targetSelectButtonUI.combatant;
        combatManager.SetState(combatManager.applyMove);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            combatManager.cameraFollow.transformToFollow = combatManager.playerCombat.transform;
            enemySelectMenuUI.DisplayMenu(false);
            combatManager.playerCombat.CombatLookDirX = previousLookDirX;
            combatManager.SetState(combatManager.styleSelectState);
        }
    }
}
