using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);

        combatManager.CombatUIManager.ChangeMenuState(combatManager.CombatUIManager.attackTargetMenu);


        combatManager.CombatUIManager.targetMenuFirstButton.Select();
        combatManager.enemies[combatManager.selectedEnemy].enemyUI.partsTargetDisplay.UpdateTargetDisplay(true, false, false);

        yield break;
    }

    public override void CombatOptionSelected(int moveValue) //triggered via Button

    {
        combatManager.enemies[combatManager.selectedEnemy].SetEnemyBodyPartTarget(moveValue);
        DisablePartsTargetDisplay();

        if (combatManager.allies.Count > 0)
        {
            combatManager.SetState(combatManager.allyMoveState);
        }

        else
        {
            combatManager.SetState(combatManager.applyMove); 
        }
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            DisablePartsTargetDisplay();

            if (combatManager.battleScheme.enemies.Count == 1)

            {
                combatManager.SetState(combatManager.secondMove);
            }

            else
            {
                combatManager.SetState(combatManager.enemySelect);
            }
        }
    }

    void DisablePartsTargetDisplay()
    {
        combatManager.enemies[combatManager.selectedEnemy].enemyUI.partsTargetDisplay.UpdateTargetDisplay(false, false, false);
    }
}

