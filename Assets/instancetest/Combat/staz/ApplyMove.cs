using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMove : State
{
    public ApplyMove(CombatManagerV3 _combatManagerV3) : base(_combatManagerV3)

    {
    }

    int test;

    void IsEnemyDead(bool _enemyIsDead)

    {
        combatManagerV3.enemyIsDead = _enemyIsDead;
    }

    public override IEnumerator Start()
    {
        CombatEvents.EnemyIsDead += IsEnemyDead;

        combatManagerV3.combatUIScript.HideTargetMenu();
        combatManagerV3.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

        CombatEvents.ShowHideFendDisplay.Invoke(true);
        CombatEvents.HighlightBodypartTarget.Invoke(false, false, false);
        CombatEvents.UpdateNarrator.Invoke(combatManagerV3.playerMoveManager.moveForNarrator);
        CombatEvents.UpdatePlayerPot.Invoke(Mathf.CeilToInt(combatManagerV3.playerStats.playerPotentialMoveCost));

        if (combatManagerV3.playerMoveManager.firstMoveIs == 1 || combatManagerV3.playerMoveManager.secondMoveIs == 1)
        {
            CombatEvents.UpdatePlayerPosition.Invoke(new Vector2(combatManagerV3.enemyGameObject.transform.position.x - 0.3f, combatManagerV3.enemyGameObject.transform.position.y), 0.5f);
            yield return new WaitForSeconds(1);
            CombatEvents.UpdatePlayerPosition.Invoke(combatManagerV3.playerFightingPosition.transform.position, 0.5f);
            CombatEvents.CalculateEnemyDamageTaken.Invoke(combatManagerV3.playerStats.attackPower);
        }

        CombatEvents.UpdateNarrator.Invoke("");

        if (combatManagerV3.enemyIsDead) 
        
        {
            yield return new WaitForSeconds(1);
            combatManagerV3.SetBattleStateVictory();
        }

        else 
        {
            yield return new WaitForSeconds(1);
            combatManagerV3.SetBattleStateEnemyAttack();
        }

        CombatEvents.EnemyIsDead -= IsEnemyDead;

    }

}
