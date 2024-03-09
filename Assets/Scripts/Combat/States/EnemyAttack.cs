using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject enemyAttackDisplay;
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

        combatManager.combatUIScript.HideTargetMenu();

        enemyAttackDisplay.SetActive(false);

        combatManager.UpdateFighterPosition(combatManager.battleScheme.enemyGameObject, new Vector2(combatManager.battleScheme.playerFightingPosition.transform.position.x + 0.3f, combatManager.battleScheme.playerFightingPosition.transform.position.y), 0.5f);
        yield return new WaitForSeconds(0.5f);

        combatManager.combatUIScript.fendScript.ApplEnemyAttackToFend(combatManager.enemy.EnemyAttackTotal());

        CombatEvents.UpdatePlayerHP.Invoke(-fendScript.attackRemainder);
        CombatEvents.AnimatorTrigger.Invoke("deflect");

        yield return new WaitForSeconds(0.5f);

        combatManager.UpdateFighterPosition(combatManager.battleScheme.enemyGameObject, combatManager.battleScheme.enemyFightingPosition.transform.position, 0.5f);

        yield return new WaitForSeconds(0.5f);

        combatManager.combatUIScript.playerDamageTakenDisplay.DisablePlayerDamageDisplay();
        combatManager.SetState(combatManager.roundReset);

    }

}

