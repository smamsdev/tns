using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMove : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject enemyAttackDisplay;

    void IsEnemyDead(bool _enemyIsDead)

    {
        combatManager.enemyIsDead = _enemyIsDead;
    }

    public override IEnumerator StartState()
    {
        CombatEvents.EnemyIsDead += IsEnemyDead;

        if (!combatManager.combatUIScript.secondMoveMenu.activeSelf)
        {
            combatManager.combatUIScript.secondMoveMenu.SetActive(false);
        }

        combatManager.combatUIScript.HideTargetMenu();
        enemyAttackDisplay.SetActive(false);
        CombatEvents.HighlightBodypartTarget?.Invoke(false, false, false);

        //   var equippedGear = combatManager.player.GetComponent<EquippedGear>().equippedGear;
        //  int i;
        //
        //  for (i = 0; i < equippedGear.Length;)
        //
        //  {
        //      equippedGear[i].ApplyAttackGear();
        //      i++;
        //  }



        combatManager.playerMoveManager.CombineMoves();

        CombatEvents.UpdatePlayerPot.Invoke(Mathf.CeilToInt(combatManager.playerStats.playerPotentialMoveCost));

        if (combatManager.playerMoveManager.firstMoveIs == 1 || combatManager.playerMoveManager.secondMoveIs == 1)
        {
            combatManager.UpdateFighterPosition(combatManager.player, new Vector2(combatManager.battleScheme.enemyGameObject.transform.position.x - 0.3f, combatManager.battleScheme.enemyGameObject.transform.position.y), 0.5f);
            yield return new WaitForSeconds(1);
            combatManager.UpdateFighterPosition(combatManager.player, combatManager.battleScheme.playerFightingPosition.transform.position, 0.5f);
            CombatEvents.CalculateEnemyDamageTaken.Invoke(combatManager.playerStats.attackPower);
            yield return new WaitForSeconds(1);
        }

        if (combatManager.enemyIsDead) 
        
        {
            yield return new WaitForSeconds(1);
            combatManager.SetState(combatManager.victory);
        }

        else 
        {

            combatManager.combatUIScript.fendScript.ShowHideFendDisplay(true);
            combatManager.combatUIScript.fendScript.UpdateFendText(combatManager.playerStats.TotalPlayerMovePower("fend"));
            yield return new WaitForSeconds(0.5f);
            combatManager.SetState(combatManager.enemyAttack);
        }

        CombatEvents.EnemyIsDead -= IsEnemyDead;

        yield return new WaitForSeconds(1);
        CombatEvents.UpdateNarrator.Invoke("");

    }

}
