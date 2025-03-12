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
        var animator = ally.GetComponent<Animator>();
        ally.allyUI.allyDamageTakenDisplay.DisableAllyDamageDisplay();

        yield return AllyMoveToAttackPos(combatManager);

        combatManager.cameraFollow.transformToFollow = ally.enemyToAttack.gameObject.transform;

        var allyLookDirection = ally.GetComponent<MovementScript>().lookDirection;
        //CombatEvents.ApplyEnemyAttackToFend(ally.AllyAttackTotal(), allyLookDirection, ally.moveSelected.attackPushStrength);
        animator.SetTrigger("Attack");

        yield return AllyReturn();

        animator.SetTrigger("CombatIdle");
        yield return null;
    }
}
