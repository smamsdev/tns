using UnityEngine;

public class PawnStateAdvancing : PawnState
{
    void MoveToTarget()
    {
        Vector2 direction = pawn.defaultAdvanceVector;
        Vector2 nudge = new Vector2(0, pawn.verticalNudge);

        pawn.rb.linearVelocity = direction * pawn.moveSpeed + nudge;
        pawn.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void EnterState()
    {
        pawn.fov.enabled = false;
        pawn.combatRange.enabled = false;

        pawn.animator.Play("Moving");

        pawn.fov.enabled = true;
        pawn.combatRange.enabled = true;
    }

    public override void PawnUpdate()
    {
        MoveToTarget();
    }
}