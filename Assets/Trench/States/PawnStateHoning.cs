using UnityEngine;

public class PawnStateHoning : PawnState
{
    public override void EnterState()
    {
        pawn.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Pawn>() == pawn.enemyPawnTarget)
        {
            pawn.TargetLost();
        }
    }

    public override void PawnUpdate()
    {
        MoveToTarget();
    }
}
