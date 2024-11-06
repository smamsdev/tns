using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMove : State
{
    [SerializeField] CombatManager combatManager;

    void IsEnemyDead(bool _enemyIsDead)

    {
        combatManager.enemyIsDead = _enemyIsDead;
    }

    public override IEnumerator StartState()
    {
        CombatEvents.EnemyIsDead += IsEnemyDead;

        combatManager.CombatUIManager.ChangeMenuState(false);

        foreach (Enemy enemy in combatManager.enemy)

        {
            enemy.enemyUI.enemyDamageTakenDisplay.DisableEnemyDamageDisplay();
            enemy.enemyUI.enemyAttackDisplay.ShowAttackDisplay(false);
        }


        //   var equippedGear = combatManager.player.GetComponent<EquippedGear>().equippedGear;
        //  int i;
        //
        //  for (i = 0; i < equippedGear.Length;)
        //
        //  {
        //      equippedGear[i].ApplyAttackGear();
        //      i++;
        //  }

        combatManager.playerCombatStats.TotalPlayerAttackPower(combatManager.selectedPlayerMove.attackMoveMultiplier);
        CombatEvents.UpdateNarrator.Invoke(combatManager.selectedPlayerMove.moveName);

        StartCoroutine(combatManager.selectedPlayerMove.OnApplyMove(combatManager, combatManager.enemy[combatManager.selectedEnemy]));

        yield return null;
    }

    public void EndMove()

    {
        StartCoroutine(EndMoveCoro());
    }

    IEnumerator EndMoveCoro()

    {
        yield return new WaitForSeconds(1f);

        foreach (Enemy enemy in combatManager.enemy)

        {
            enemy.enemyUI.enemyFendScript.enemyFendAnimator.SetTrigger("fendFade");
            enemy.enemyUI.enemyStatsDisplay.enemyStatsDisplayGameObject.SetActive(false);
        }

        if (combatManager.enemyIsDead)

        {
            combatManager.SetState(combatManager.victory);
            CombatEvents.EnemyIsDead -= IsEnemyDead;
        }

        else
        {
            CombatEvents.UpdatePlayerPot.Invoke(combatManager.selectedPlayerMove.potentialChange);
            combatManager.playerCombatStats.TotalPlayerFendPower(combatManager.selectedPlayerMove.fendMoveMultiplier);
            combatManager.CombatUIManager.playerFendScript.UpdateFendText(combatManager.playerCombatStats.playerFend);

        if (combatManager.playerCombatStats.playerFend >0)
        {
            combatManager.CombatUIManager.playerFendScript.ShowFendDisplay(true);
            yield return new WaitForSeconds(1f);
        }

            combatManager.SetState(combatManager.enemyAttack);

            CombatEvents.EnemyIsDead -= IsEnemyDead;
        }
    }

    private void OnDisable()
    {
        CombatEvents.EnemyIsDead -= IsEnemyDead;
    }
}
