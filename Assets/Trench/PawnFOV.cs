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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if target is outside FOV remove target and change state
        if (collision.CompareTag("PawnBody") && collision.GetComponent<Pawn>() == pawn.enemyPawnTarget)
        {
            Debug.Log("leaving" + collision.gameObject.name);
            pawn.enemyPawnTarget = null;
            pawn.EnterState(pawn.advancing);
        }
    }
}