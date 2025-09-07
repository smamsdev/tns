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
        combatManager.combatMenuManager.thirdMenuFirstButton = selectEnemyMenuScript.defaultButton;
        combatManager.combatMenuManager.thirdMenuFirstButton.Select();
        previousLookDir = combatManager.playerCombat.movementScript.lookDirection;

        yield break;
    }

    public void CombatantSelected(Combatant combatant)
    {
        combatManager.playerCombat.targetToAttack = combatant;
        combatManager.combatMenuManager.thirdMenuFirstButton = buttonSelected;
        selectEnemyMenuScript.DeselectEnemy(selectEnemyMenuScript.enemyhighlighted);
        combatManager.SetState(combatManager.applyMove);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManager.cameraFollow.transformToFollow = combatManager.player.transform;
            selectEnemyMenuScript.DeselectEnemy(selectEnemyMenuScript.enemyhighlighted);
            combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.enemySelectMenu, false);
            combatManager.combatMenuManager.SetButtonNormalColor(combatManager.secondMove.buttonSelected, Color.white);

            combatManager.playerCombat.movementScript.lookDirection = previousLookDir;

            combatManager.SetState(combatManager.secondMove);

        }
    }
}
