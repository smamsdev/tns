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
        CombatEvents.MeleeAttack += MeleeAttack;
        CombatEvents.EndMove += EndMove;

        combatManager.combatUIScript.selectEnemyMenuScript.ShowEnemySelectMenu(false);

        foreach (Enemy enemy in combatManager.enemy)

        {
            enemy.enemyUI.enemyDamageTakenDisplay.DisableEnemyDamageDisplay();
            enemy.enemyUI.enemyAttackDisplay.ShowAttackDisplay(false);
        }

        if (!combatManager.combatUIScript.secondMoveMenu.activeSelf)
        {
            combatManager.combatUIScript.secondMoveMenu.SetActive(false);
        }

        combatManager.combatUIScript.HideTargetMenu();
        combatManager.enemy[combatManager.selectedEnemy].enemyUI.partsTargetDisplay.UpdateTargetDisplay(false, false, false);

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

        combatManager.selectedPlayerMove.OnApplyMove();

        yield return null;//
    }

    public void MeleeAttack()

    {
        StartCoroutine(MeleeAttackCoroutine());
    }

    IEnumerator MeleeAttackCoroutine()

    {
            combatManager.UpdateFighterPosition(combatManager.player, new Vector2(combatManager.battleScheme.enemyGameObject[combatManager.selectedEnemy].transform.position.x - 0.3f, combatManager.battleScheme.enemyGameObject[combatManager.selectedEnemy].transform.position.y), 0.5f);
            yield return new WaitForSeconds(0.5f);
            combatManager.enemy[combatManager.selectedEnemy].enemyUI.enemyFendScript.ApplyPlayerAttackToFend(combatManager.playerCombatStats.attackPower);

            yield return new WaitForSeconds(0.3f);
            combatManager.UpdateFighterPosition(combatManager.player, combatManager.battleScheme.playerFightingPosition.transform.position, 0.5f);

            yield return new WaitForSeconds(1);

            EndMove();
    }

    void EndMove()

    {
        StartCoroutine(EndMoveCoro());
    }

    IEnumerator EndMoveCoro()

    {
        foreach (Enemy enemy in combatManager.enemy)

        {
            enemy.enemyUI.enemyFendScript.enemyFendAnimator.SetTrigger("fendFade");
        }

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
