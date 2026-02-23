using UnityEngine;

public class PawnEngagement : MonoBehaviour
{
    public Pawn pawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyArmy")
        {
            Debug.Log("attacking");
            pawn.Engaging(collision.gameObject.transform);
        }
    }
}
