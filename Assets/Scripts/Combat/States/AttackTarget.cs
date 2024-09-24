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
        combatManager.enemy[combatManager.selectedEnemy].enemyUI.partsTargetDisplay.UpdateTargetDisplay(true, false, false);

        yield break;
    }

    public override void CombatOptionSelected(int moveValue)

    {
        combatManager.enemy[combatManager.selectedEnemy].SetEnemyBodyPartTarget(moveValue);
        DisablePartsTargetDisplay();
        combatManager.SetState(combatManager.applyMove);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            DisablePartsTargetDisplay();

            if (combatManager.battleScheme.enemyGameObject.Length == 1)

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
        combatManager.enemy[combatManager.selectedEnemy].enemyUI.partsTargetDisplay.UpdateTargetDisplay(false, false, false);
    }
}

