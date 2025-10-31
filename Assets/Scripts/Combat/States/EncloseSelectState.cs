using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncloseSelectState : State
{
    public SelectEnemyMenuScript selectEnemyMenuScript;
    int previousLookDirX;

    public override IEnumerator StartState()
    {
        previousLookDirX = combatManager.playerCombat.CombatLookDirX;
        selectEnemyMenuScript.InitializeButtonSlots();
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.enemySelectMenu, true);
        combatManager.combatMenuManager.selectEnemyMenuDefaultButton.Select();
        yield break;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManager.cameraFollow.transformToFollow = combatManager.playerCombat.transform;
            combatManager.tacticalSelectState.isEnclosing = false;
            selectEnemyMenuScript.DeselectEnemy(selectEnemyMenuScript.enemySelectButtonScriptHighlighted);
            combatManager.combatMenuManager.SetButtonNormalColor(combatManager.tacticalSelectState.lastButtonSelected, Color.white);
            combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.enemySelectMenu, false);
            combatManager.tacticalSelectState.lastButtonSelected.Select();
            combatManager.playerCombat.CombatLookDirX = previousLookDirX;
            combatManager.SetState(combatManager.tacticalSelectState);
        }
    }
}
