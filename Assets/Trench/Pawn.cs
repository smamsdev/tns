using UnityEngine;

public class Pawn : MonoBehaviour
{
    public Transform enemyPawnTarget;
    public Transform engaging;
    public float moveSpeed = 5f;
    public GameObject enemyBase;
    public bool attacking;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    Vector2 TargetPosition()
    {
        Vector3 targetPosition;

        if (enemyPawnTarget == null)
            targetPosition = new Vector3 (enemyBase.transform.position.x, this.transform.position.y);

        else
            targetPosition = enemyPawnTarget.transform.position;

        return targetPosition;
    }

    void MoveToTarget()
    {
        Vector2 direction = (TargetPosition() - rb.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    public void Engaging(Transform targetToEngage)
    { 
        engaging = targetToEngage;
        attacking = true;
        rb.bodyType = RigidbodyType2D.Static;
    }

    void FixedUpdate()
    {
        if (!attacking)
            MoveToTarget();

    }
}