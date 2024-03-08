using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject enemyAttackDisplay;

    public override IEnumerator StartState()
    {

        var equippedGear = combatManager.player.GetComponent<GearEquip>().equippedGear;
        int i;

        for (i = 0; i < equippedGear.Length;)

        {
            equippedGear[i].ApplyFendGear();
            i++;
        }

        combatManager.combatUIScript.HideTargetMenu();

        enemyAttackDisplay.SetActive(false);

        combatManager.UpdateFighterPosition(combatManager.battleScheme.enemyGameObject, new Vector2(combatManager.battleScheme.playerFightingPosition.transform.position.x + 0.3f, combatManager.battleScheme.playerFightingPosition.transform.position.y), 0.5f);
        yield return new WaitForSeconds(0.5f);
        
      //  CombatEvents.UpdateFendDisplay?.Invoke(combatManager.playerStats.playerFend - combatManager.enemyRawAttackPower);
        CombatEvents.ATTACKTOAPPLY?.Invoke(combatManager.enemyRawAttackPower);
        int enemyAttackPower = Mathf.Clamp(combatManager.enemyRawAttackPower - combatManager.playerStats.playerFend, 0, 9999);
        CombatEvents.UpdatePlayerHP.Invoke(enemyAttackPower);
        CombatEvents.AnimatorTrigger.Invoke("deflect");

        yield return new WaitForSeconds(0.5f);


        combatManager.UpdateFighterPosition(combatManager.battleScheme.enemyGameObject, combatManager.battleScheme.enemyFightingPosition.transform.position, 0.5f);

        yield return new WaitForSeconds(0.5f);



        combatManager.SetState(combatManager.roundReset);
    }

}

