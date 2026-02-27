using UnityEngine;

public class PawnStateDefeat : PawnState
{
    public override void EnterState()
    {
        pawn.rb.linearVelocity = Vector2.zero;
        pawn.rb.angularVelocity = 0f;
        pawn.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        pawn.rb.simulated = false;

        pawn.physicalCollider.enabled = false;
        pawn.animator.Play("Fall");
    }

    public override void PawnUpdate()
    {
        //
    }
}
