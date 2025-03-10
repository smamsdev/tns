using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyBasicMove : AllyMove
{
    public int rng;

    public override void LoadMove(Ally _ally)
    {
        ally = _ally;

        ally.attackTotal = Mathf.RoundToInt(ally.attackBase * attackMoveModPercent);
        ally.fendTotal = Mathf.RoundToInt(ally.fendBase * fendMoveModPercent);

        rng = Mathf.RoundToInt(ally.attackTotal * Random.Range(-0.3f, 0.3f));

        ally.attackTotal = Mathf.RoundToInt(ally.attackTotal) + rng;
    }

    public override IEnumerator AllyAttack(CombatManager _combatManager)

    {
        combatManager = _combatManager;
        var distance = ally.moveSelected.distanceToCoverPercent;

        ally.allyUI.allyDamageTakenDisplay.DisableAllyDamageDisplay();

        var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
        var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
        yield return (combatMovementInstance.MoveCombatant(ally.gameObject, combatManager.player.transform.position, stoppingPercentage: distance));
        Destroy(combatMovementInstanceGO);

        combatManager.cameraFollow.transformToFollow = combatManager.player.transform;

        var allyLookDirection = ally.GetComponent<MovementScript>().lookDirection;

        CombatEvents.ApplyEnemyAttackToFend(ally.AllyAttackTotal(), allyLookDirection, ally.moveSelected.attackPushStrength);
        yield return null;
    }
}
