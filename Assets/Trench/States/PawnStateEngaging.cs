using UnityEngine;

public class PawnStateEngaging : PawnState
{
    public override void EnterState()
    {

    }

    void MoveToTarget()
    {
        if (pawn.enemyPawnTarget == null || pawn.enemyPawnTarget.hp <= 0)
        {
            pawn.EnterState(pawn.advancing);
            return;
        }

        Vector2 direction = ((Vector2)pawn.enemyPawnTarget.transform.position - pawn.rb.position).normalized;
        Vector2 nudge = new Vector2(0, pawn.verticalNudge);

        pawn.rb.linearVelocity = direction * pawn.moveSpeed + nudge;
        pawn.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void PawnUpdate()
    {
        MoveToTarget();
    }
}
