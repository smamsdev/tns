using UnityEngine;

public class PawnPOV : MonoBehaviour
{
    public Pawn pawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("detectedsomething");
        if (collision.tag == "EnemyArmy")
        {
            Debug.Log("detectedenemy");
            pawn.enemyPawnTarget = collision.gameObject.transform;
        }
    }
}
