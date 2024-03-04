using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] CombatManagerV3 combatManagerV3;
    [SerializeField] GameObject enemyAttackDisplay;

    public IEnumerator StartState()
    {

        var equippedGear = combatManagerV3.player.GetComponent<GearEquip>().equippedGear;
        int i;

        for (i = 0; i < equippedGear.Length;)

        {
            equippedGear[i].ApplyFendGear();
            i++;
        }

        combatManagerV3.combatUIScript.HideTargetMenu();

        enemyAttackDisplay.SetActive(false);

        combatManagerV3.UpdateFighterPosition(combatManagerV3.battleScheme.enemyGameObject, new Vector2(combatManagerV3.battleScheme.playerFightingPosition.transform.position.x + 0.3f, combatManagerV3.battleScheme.playerFightingPosition.transform.position.y), 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        CombatEvents.UpdateFendDisplay?.Invoke(combatManagerV3.playerStats.playerFend - combatManagerV3.enemyRawAttackPower);
        int enemyAttackPower = Mathf.Clamp(combatManagerV3.enemyRawAttackPower - combatManagerV3.playerStats.playerFend, 0, 9999);
        CombatEvents.UpdatePlayerHP.Invoke(enemyAttackPower);

        yield return new WaitForSeconds(0.5f);
        
        combatManagerV3.UpdateFighterPosition(combatManagerV3.battleScheme.enemyGameObject, combatManagerV3.battleScheme.enemyFightingPosition.transform.position, 0.5f);

        yield return new WaitForSeconds(0.5f);

        combatManagerV3.SetBattleStateRoundReset();
    }

}

