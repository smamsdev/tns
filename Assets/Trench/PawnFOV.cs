using UnityEngine;

public class PawnFOV : MonoBehaviour
{
    public Pawn pawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("PawnBody")) return;

        Pawn pawnCollision = collision.GetComponent<Pawn>();
        if (pawnCollision == null) return;

        if (pawnCollision.team != pawn.team)
        {
            pawn.TargetDetected(pawnCollision);
            pawn.fov.enabled = false;
        }
    }
}