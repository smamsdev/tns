using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelectState : State
{
    public EnemySelectMenuUI enemySelectMenuUI;
    public int previousLookDirX;

    public override IEnumerator StartState()
    {
        previousLookDirX = combatManager.playerCombat.CombatLookDirX;
        enemySelectMenuUI.InitializeButtonSlots(combatManager.enemies);
        enemySelectMenuUI.DisplayMenu(true);

        //i think you need the frame yield here to not mess up the lookDir snapshot
        yield return null;
        enemySelectMenuUI.menuButtons[enemySelectMenuUI.highlightedButtonIndex].Select();
    }

    public void CombatantSelected(TargetSelectButtonUI targetSelectButtonUI)
    {
        Combatant selectedCombatant = targetSelectButtonUI.combatant;

        combatManager.playerCombat.CombatLookDirX = previousLookDirX;
        combatManager.playerCombat.targetCombatant = targetSelectButtonUI.combatant;
        combatManager.SetState(combatManager.applyMoveState);
        TargetUnHighlighted(targetSelectButtonUI.combatant);
        enemySelectMenuUI.highlightedButtonIndex = 0;
    }

    public void TargetHighlighted(TargetSelectButtonUI targetSelectButtonUI)
    {
        enemySelectMenuUI.highlightedButtonIndex = enemySelectMenuUI.targetSelectButtonUIs.IndexOf(targetSelectButtonUI);

        combatManager.cameraFollow.transformToFollow = targetSelectButtonUI.combatant.transform;
        var combatantUI = targetSelectButtonUI.combatant.combatantUI;
        combatantUI.statsDisplay.ShowStatsDisplay(true);

        combatantUI.selectedAnimator.SetBool("Flash", true);
        targetSelectButtonUI.combatant.combatantUI.DisplayCombatantMove(targetSelectButtonUI.combatant);

        Vector2 direction = (targetSelectButtonUI.combatant.transform.position - combatManager.playerCombat.transform.position).normalized;
        combatManager.playerCombat.CombatLookDirX = (int)Mathf.Sign(direction.x);
    }

    public void TargetUnHighlighted(Combatant combatant)
    {
        var combatantUI = combatant.combatantUI;

        combatantUI.selectedAnimator.SetBool("Flash", false);
        combatantUI.statsDisplay.ShowStatsDisplay(false);
        combatantUI.attackDisplay.ShowAttackDisplay(combatant, false);
        combatantUI.fendScript.ShowFendDisplay(combatant, false);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TargetUnHighlighted(enemySelectMenuUI.targetSelectButtonUIs[enemySelectMenuUI.highlightedButtonIndex].combatant);
            combatManager.cameraFollow.transformToFollow = combatManager.playerCombat.transform;
            enemySelectMenuUI.DisplayMenu(false);
            combatManager.playerCombat.CombatLookDirX = previousLookDirX;
            combatManager.SetState(combatManager.actionSelectState);
        }
    }
}
