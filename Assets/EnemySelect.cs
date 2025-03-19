using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelect : State
{
    public SelectEnemyMenuScript selectEnemyMenuScript;
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        if (combatManager.enemies.Count == 1)

        {
            combatManager.SetState(combatManager.attackTarget);
            var enemy = combatManager.enemies[0];
            combatManager.playerCombat.targetToAttack = enemy;
            enemy.combatantUI.statsDisplay.statsDisplayGameObject.SetActive(true);
            combatManager.cameraFollow.transformToFollow = enemy.transform;
            yield break;
        }

        combatManager.CombatUIManager.selectEnemyMenuScript.InitializeButtonSlots();
        combatManager.CombatUIManager.ChangeMenuState(combatManager.CombatUIManager.enemySelectMenu);
        combatManager.CombatUIManager.thirdMenuFirstButton.Select();

        yield break;
    }

    public override void CombatOptionSelected(int moveValue)

    {
        combatManager.SetState(combatManager.attackTarget);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManager.cameraFollow.transformToFollow = combatManager.player.transform;
            combatManager.SetState(combatManager.secondMove);
        }
    }
}
