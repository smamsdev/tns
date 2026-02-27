using UnityEngine;

public class PawnStateCombat : PawnState
{
    public float attackCoolDown;
    public float timer;

    public override void EnterState()
    {
        float rng = Random.Range(0.02f, 0.2f);
        timer = 0 + rng;

        pawn.rb.linearVelocity = Vector2.zero;
        pawn.rb.angularVelocity = 0f;
        pawn.rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void HandleAttackState()
    {
        if (pawn.enemyPawnTarget == null || pawn.enemyPawnTarget.hp <= 0)
        {
            pawn.EnterState(pawn.advancing);
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0f && pawn.enemyPawnTarget.hp > 0 && pawn.hp > 0)
        {
            pawn.animator.Play("Attack");
        }
    }

    public override void PawnUpdate()
    {
        HandleAttackState();
    }
}
