using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMove : State
{
    [SerializeField] CombatManagerV3 combatManagerV3;

    void IsEnemyDead(bool _enemyIsDead)

    {
        combatManagerV3.enemyIsDead = _enemyIsDead;
    }

    public override IEnumerator StartState()
    {
        CombatEvents.EnemyIsDead += IsEnemyDead;

        if (!combatManagerV3.combatUIScript.secondMoveMenu.activeSelf)
        {
            combatManagerV3.combatUIScript.secondMoveMenu.SetActive(false);
        }

        combatManagerV3.combatUIScript.HideTargetMenu();
        CombatEvents.HighlightBodypartTarget?.Invoke(false, false, false);

        var equippedGear = combatManagerV3.player.GetComponent<GearEquip>().equippedGear;
        int i;

        for (i = 0; i < equippedGear.Length;)

        {
            equippedGear[i].ApplyAttackGear();
            i++;
        }

        combatManagerV3.playerMoveManager.CombineMoves();

        CombatEvents.UpdateNarrator.Invoke(combatManagerV3.playerMoveManager.moveForNarrator);
        CombatEvents.UpdatePlayerPot.Invoke(Mathf.CeilToInt(combatManagerV3.playerStats.playerPotentialMoveCost));

        if (combatManagerV3.playerMoveManager.firstMoveIs == 1 || combatManagerV3.playerMoveManager.secondMoveIs == 1)
        {
            CombatEvents.UpdateFighterPosition.Invoke(combatManagerV3.player, new Vector2(combatManagerV3.battleScheme.enemyGameObject.transform.position.x - 0.3f, combatManagerV3.battleScheme.enemyGameObject.transform.position.y), 0.5f);
            yield return new WaitForSeconds(1);
            CombatEvents.UpdateFighterPosition.Invoke(combatManagerV3.player, combatManagerV3.battleScheme.playerFightingPosition.transform.position, 0.5f);
            CombatEvents.CalculateEnemyDamageTaken.Invoke(combatManagerV3.playerStats.attackPower);
        }

        if (combatManagerV3.enemyIsDead) 
        
        {
            yield return new WaitForSeconds(1);
            combatManagerV3.SetState(combatManagerV3.victory);
        }

        else 
        {
            CombatEvents.ShowHideFendDisplay.Invoke(true);
            yield return new WaitForSeconds(1);
            combatManagerV3.SetState(combatManagerV3.enemyAttack);
        }

        CombatEvents.EnemyIsDead -= IsEnemyDead;

        yield return new WaitForSeconds(1);
        CombatEvents.UpdateNarrator.Invoke("");

    }

}
