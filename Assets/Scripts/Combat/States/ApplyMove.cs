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
        CombatEvents.MeleeAttack += MeleeAttack;
        CombatEvents.EndMove += EndMove;

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

        combatManager.combatUIScript.playerDamageTakenDisplay.DisablePlayerDamageDisplay();
        combatManager.combatUIScript.enemyDamageTakenDisplay.DisableEnemyDamageDisplay();

        combatManager.playerMoveManager.CombineStanceAndMove();
        combatManager.selectedPlayerMove = combatManager.playerMoveManager.GetSelectedPlayerMove();

        combatManager.playerCombatStats.TotalPlayerAttackPower(combatManager.selectedPlayerMove.attackMoveMultiplier);
        CombatEvents.UpdateNarrator.Invoke(combatManager.selectedPlayerMove.moveName);

        combatManager.selectedPlayerMove.OnApplyMove();

        yield return null;//
    }

    public void MeleeAttack()

    {
        StartCoroutine(MeleeAttackCoroutine());
    }

    IEnumerator MeleeAttackCoroutine()

    {
        if (combatManager.playerMoveManager.firstMoveIs == 1 || combatManager.playerMoveManager.secondMoveIs == 1)
        {
            combatManager.UpdateFighterPosition(combatManager.player, new Vector2(combatManager.battleScheme.enemyGameObject.transform.position.x - 0.3f, combatManager.battleScheme.enemyGameObject.transform.position.y), 0.5f);
            yield return new WaitForSeconds(0.5f);
            combatManager.combatUIScript.enemyFendScript.ApplyPlayerAttackToFend(combatManager.playerCombatStats.attackPower);

            yield return new WaitForSeconds(0.3f);
            combatManager.UpdateFighterPosition(combatManager.player, combatManager.battleScheme.playerFightingPosition.transform.position, 0.5f);

            yield return new WaitForSeconds(1);

            EndMove();
        }
    }

    void EndMove()

    {
        StartCoroutine(EndMoveCoro());
    }

    IEnumerator EndMoveCoro()

    {
        yield return new WaitForSeconds(0.5f);

        if (combatManager.enemyIsDead)

        {
            yield return new WaitForSeconds(1);
            combatManager.SetState(combatManager.victory);
        }

        else
        {
            combatManager.SetState(combatManager.enemyAttack);
        }

        CombatEvents.EnemyIsDead -= IsEnemyDead;
        CombatEvents.MeleeAttack -= MeleeAttack;
        CombatEvents.EndMove -= EndMove;

        yield return new WaitForSeconds(1);
        CombatEvents.UpdateNarrator.Invoke("");
    }

    private void OnDisable()
    {
        CombatEvents.EnemyIsDead -= IsEnemyDead;
        CombatEvents.MeleeAttack -= MeleeAttack;
        CombatEvents.EndMove -= EndMove;
    }
}
