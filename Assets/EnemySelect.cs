using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelect : State
{
    public SelectEnemyMenuScript selectEnemyMenuScript;

    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.selectEnemyMenuScript.InitializeButtonSlots();
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.enemySelectMenu, true);
        combatManager.combatMenuManager.thirdMenuFirstButton = buttonSelected;
        combatManager.combatMenuManager.thirdMenuFirstButton.Select();

        // if (combatManager.enemies.Count == 1)
        //
        // {
        //     combatManager.SetState(combatManager.attackTarget);
        //     var enemy = combatManager.enemies[0];
        //     combatManager.playerCombat.targetToAttack = enemy;
        //     enemy.combatantUI.statsDisplay.statsDisplayGameObject.SetActive(true);
        //     combatManager.cameraFollow.transformToFollow = enemy.transform;
        // }

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

            combatManager.SetState(combatManager.secondMove);

        }
    }
}
