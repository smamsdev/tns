using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolentBasic : ViolentMove
{

    public override IEnumerator OnEnemyAttack(CombatManager _combatManager, Enemy _enemy)

    {
        yield break; //null as it's a basic move only
    }

    public override IEnumerator Return()
    {
        var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
        var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
        yield return (combatMovementInstance.MoveCombatant(combatManager.player.gameObject, combatManager.battleScheme.playerFightingPosition.transform.position));
        Destroy(combatMovementInstanceGO);
    }
}
