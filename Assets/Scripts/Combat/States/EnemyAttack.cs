using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : State
{
    public EnemyAttack(CombatManagerV3 _combatManagerV3) : base(_combatManagerV3)

    {
    }

    public override IEnumerator Start()
    {

        var equippedGear = combatManagerV3.player.GetComponent<GearEquip>().equippedGear;
        int i;

        for (i = 0; i < equippedGear.Length;)

        {
            equippedGear[i].ApplyFendGear();
            i++;
        }

        int enemyAttackPower = Mathf.Clamp(combatManagerV3.enemyRawAttackPower - combatManagerV3.playerStats.playerFend, 0, 9999);
        CombatEvents.UpdatePlayerHP.Invoke(enemyAttackPower);

        combatManagerV3.combatUIScript.HideTargetMenu();
        CombatEvents.ShowHideFendDisplay.Invoke(true);

        combatManagerV3.UpdateFighterPosition(combatManagerV3.battleScheme.enemyGameObject, new Vector2(combatManagerV3.battleScheme.playerFightingPosition.transform.position.x + 0.3f, combatManagerV3.battleScheme.playerFightingPosition.transform.position.y), 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        CombatEvents.UpdateFendDisplay?.Invoke(combatManagerV3.playerStats.playerFend - combatManagerV3.enemyRawAttackPower);
        
        yield return new WaitForSeconds(0.5f);
        
        combatManagerV3.UpdateFighterPosition(combatManagerV3.battleScheme.enemyGameObject, combatManagerV3.battleScheme.enemyFightingPosition.transform.position, 0.5f);

        combatManagerV3.SetBattleStateRoundReset();
    }

    public override void Update()
    {

    }

}

