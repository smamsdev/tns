using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundReset : State
{
    public override IEnumerator StartState()
    {
        combatManager.playerCombat.combatantUI.fendScript.ShowFendDisplay(combatManager.playerCombat, false);
        combatManager.combatMenuManager.SetButtonNormalColor(combatManager.combatMenuManager.firstMenuFirstButton, Color.white);
        combatManager.combatMenuManager.SetButtonNormalColor(combatManager.combatMenuManager.secondMenuFirstButton, Color.white);
        if (combatManager.combatMenuManager.thirdMenuFirstButton != null)
        {
            combatManager.combatMenuManager.SetButtonNormalColor(combatManager.combatMenuManager.thirdMenuFirstButton, Color.white);
        }

        foreach (Enemy enemy in combatManager.enemies)
        {
            combatManager.SelectTargetToAttack(enemy, combatManager.allAlliesToTarget);

            if (enemy.GetComponent<MovementScript>().lookDirection == Vector2.left)
            {
                combatManager.SetUIBasedOnLookDir(enemy);
            }

            combatManager.SelectAndDisplayCombatantMove(enemy);
            yield return new WaitForSeconds(.7f);
            enemy.combatantUI.attackDisplay.attackDisplayAnimator.Play("CombatantAttackDamageFadeDown");
            if (enemy.fendTotal > 0)
            {
                enemy.combatantUI.fendScript.fendAnimator.Play("FendFade");
            }
        }

        foreach (Ally ally in combatManager.allies)
        {
            combatManager.SelectTargetToAttack(ally, combatManager.enemies);

            if (ally.GetComponent<MovementScript>().lookDirection == Vector2.left)
            {
                combatManager.SetUIBasedOnLookDir(ally);
            }

            combatManager.SelectAndDisplayCombatantMove(ally);
            yield return new WaitForSeconds(.7f);
            ally.combatantUI.attackDisplay.attackDisplayAnimator.Play("CombatantAttackDamageFadeDown");
            if (ally.fendTotal > 0)
            {
                ally.combatantUI.fendScript.fendAnimator.Play("FendFade");
            }
        }

        yield return new WaitForSeconds(0.5f);

        combatManager.cameraFollow.transformToFollow = combatManager.player.transform;
        combatManager.roundCount++;
        combatManager.playerCombat.playerMoveManager.firstMoveIs = 0;
        combatManager.playerCombat.playerMoveManager.secondMoveIs = 0;
        combatManager.playerCombat.attackTotal = 0;
        combatManager.playerCombat.fendTotal = 0;
        combatManager.playerCombat.combatantUI.fendScript.fendTextMeshProUGUI.text = "0";
        combatManager.SetState(combatManager.firstMove);
        yield break;
    }
}
