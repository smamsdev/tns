using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : State
{
    [SerializeField] CombatManager combatManager;
    public Gear[] equippedGear;
    [SerializeField] FendScript fendScript;
    public int enemyIteration = 0;

    public override IEnumerator StartState()
    {
        CombatEvents.CounterAttack += CounterAttack;

        //   equippedGear = combatManager.player.GetComponent<GearEquip>().equippedGear;
        //   int i;
        //
        //   for (i = 0; i < equippedGear.Length;)
        //
        //   {
        //       equippedGear[i].ApplyFendGear();
        //       i++;
        //   }

        combatManager.combatUIScript.HideTargetMenu();


        combatManager.playerCombatStats.TotalPlayerFendPower(combatManager.selectedPlayerMove.fendMoveMultiplier);

        combatManager.combatUIScript.playerFendScript.UpdateFendText(combatManager.playerCombatStats.playerFend);
        combatManager.combatUIScript.playerFendScript.ShowFendDisplay(true);

        CombatEvents.UpdatePlayerPot.Invoke(combatManager.playerCombatStats.currentPotential + combatManager.selectedPlayerMove.potentialChange);

        yield return new WaitForSeconds(1.0f);

        if (combatManager.enemy[combatManager.selectedEnemy].attackTotal == 0 && combatManager.enemy[combatManager.selectedEnemy].fendTotal > 0)
        {

            combatManager.enemy[combatManager.selectedEnemy].enemyUI.enemyFendScript.ShowFendDisplay(false);
            combatManager.combatUIScript.enemyDamageTakenDisplay.EnemyDamageTakenTextMeshProUGUI.enabled = false;

        }

        else if (combatManager.enemy[combatManager.selectedEnemy].attackTotal > 0)

        {

            combatManager.enemy[combatManager.selectedEnemy].enemyUI.enemyFendScript.ShowFendDisplay(false);

            combatManager.enemy[enemyIteration].enemyUI.enemyDamageTakenDisplay.DisableEnemyDamageDisplay();


            combatManager.UpdateFighterPosition(combatManager.battleScheme.enemyGameObject[combatManager.selectedEnemy], new Vector2(combatManager.battleScheme.playerFightingPosition.transform.position.x + 0.3f, combatManager.battleScheme.playerFightingPosition.transform.position.y), 0.5f);

            yield return new WaitForSeconds(0.5f);

            combatManager.selectedPlayerMove.OnEnemyAttack();
            combatManager.combatUIScript.playerFendScript.ApplyEnemyAttackToFend(combatManager.enemy[combatManager.selectedEnemy].EnemyAttackTotal());

            yield return new WaitForSeconds(0.5f);

            combatManager.UpdateFighterPosition(combatManager.battleScheme.enemyGameObject[combatManager.selectedEnemy], combatManager.enemy[combatManager.selectedEnemy].enemyFightingPosition.transform.position, 0.5f);

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

        combatManager.enemy[combatManager.selectedEnemy].DamageTaken(combatManager.playerCombatStats.attackPower, combatManager.selectedPlayerMove.damageToBodyMultiplier);
    }

    private void OnDisable()
    {
        CombatEvents.CounterAttack -= CounterAttack;
    }
}

