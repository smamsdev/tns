using UnityEngine;
using static Pawn;

public class PawnCombatCollision : MonoBehaviour
{
    public Pawn pawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("PawnBody")) return;

        Pawn pawnCollision = collision.GetComponent<Pawn>();

        if (pawnCollision == null) return;

        if (pawnCollision.team != pawn.team)
        {
            pawn.CombatDetected(pawnCollision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Pawn>() == pawn.enemyPawnTarget)
        {
            pawn.TargetLost();
        }
    }
}
