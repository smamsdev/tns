using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : State
{
    [SerializeField] CombatManager combatManager;
    public Gear[] equippedGear;
    [SerializeField] FendScript fendScript;

    public override IEnumerator StartState()
    {

        //   equippedGear = combatManager.player.GetComponent<GearEquip>().equippedGear;
        //   int i;
        //
        //   for (i = 0; i < equippedGear.Length;)
        //
        //   {
        //       equippedGear[i].ApplyFendGear();
        //       i++;
        //   }

        CombatEvents.CounterAttack += CounterAttack;

        combatManager.combatUIScript.HideTargetMenu();

        combatManager.playerCombatStats.TotalPlayerFendPower(combatManager.selectedPlayerMove.fendMoveMultiplier);
        combatManager.combatUIScript.playerFendScript.UpdateFendText(combatManager.playerCombatStats.playerFend);
        combatManager.combatUIScript.playerFendScript.ShowFendDisplay(true);

        CombatEvents.UpdatePlayerPot.Invoke(combatManager.playerCombatStats.currentPotential + combatManager.selectedPlayerMove.potentialChange);

        yield return new WaitForSeconds(0.5f);

        if (combatManager.enemy.attackTotal == 0 && combatManager.enemy.fendTotal > 0)
        {

            // yield return new WaitForSeconds(0.5f);

            combatManager.combatUIScript.enemyFendScript.ShowHideFendDisplay(false);
            combatManager.combatUIScript.enemyDamageTakenDisplay.EnemyDamageTakenTextMeshProUGUI.enabled = false;

        }

        else if (combatManager.enemy.attackTotal > 0)

        {
            combatManager.combatUIScript.enemyFendScript.ShowHideFendDisplay(false);
            combatManager.combatUIScript.enemyDamageTakenDisplay.EnemyDamageTakenTextMeshProUGUI.enabled = false;

            combatManager.UpdateFighterPosition(combatManager.battleScheme.enemyGameObject, new Vector2(combatManager.battleScheme.playerFightingPosition.transform.position.x + 0.3f, combatManager.battleScheme.playerFightingPosition.transform.position.y), 0.5f);

            yield return new WaitForSeconds(0.5f);

            combatManager.selectedPlayerMove.OnEnemyAttack();
            combatManager.combatUIScript.playerFendScript.ApplyEnemyAttackToFend(combatManager.enemy.EnemyAttackTotal());
            combatManager.combatUIScript.playerFendScript.FendIconAnimationState(1);

            yield return new WaitForSeconds(0.5f);

            combatManager.UpdateFighterPosition(combatManager.battleScheme.enemyGameObject, combatManager.enemy.enemyFightingPosition.transform.position, 0.5f);

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);

        CombatEvents.CounterAttack -= CounterAttack;
        combatManager.SetState(combatManager.roundReset);

    }

    void CounterAttack()

    {
        StartCoroutine(CounterAttackCoro());
    }

    IEnumerator CounterAttackCoro()

    {
        //hit *do animation or something here
        yield return new WaitForSeconds(1.0f);

        combatManager.enemy.DamageTaken(combatManager.playerCombatStats.attackPower, combatManager.selectedPlayerMove.damageToBodyMultiplier);
    }

    private void OnDisable()
    {
        CombatEvents.CounterAttack -= CounterAttack;
    }
}

