using System.Collections;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public enum Team
    {
        Player,
        Enemy
    }

    public Team team;
    [SerializeField] PawnState state;
    public GameObject targetFortress;
    public Pawn enemyPawnTarget;
    public float moveSpeed = 5f;
    public Animator animator;
    public Rigidbody2D rb;
    public float verticalNudge;
    public bool defeated;
    public Collider2D physicalCollider;

    [Header("Stats")]
    public int hp;
    public int attackPower;

    [Header("Other Colliders")]
    public Collider2D fov;
    public Collider2D combatRange;

    [Header("States")]
    public PawnState advancing;
    public PawnState engaging;
    public PawnState combat;
    public PawnState defeat;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    public void EnterState(PawnState newState)
    {
        newState.EnterState();
        state = newState;

        //if (team == Team.Player)
        //Debug.Log(this.gameObject.name + "state changed to " + state.name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TrenchBoundary"))
            return;

        float movementDirectionX = Mathf.Sign(targetFortress.transform.position.x - transform.position.x);
        float deltaX = collision.transform.position.x - transform.position.x;

        if (deltaX * movementDirectionX > 0f)
        {
            verticalNudge = collision.transform.position.y > transform.position.y ? -0.5f : 0.5f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        verticalNudge = 0;
    }

    public void TargetDetected(Pawn newTarget)
    {
        enemyPawnTarget = newTarget;
        EnterState(engaging);
    }

    public void CombatDetected(Pawn newTarget)
    {
        enemyPawnTarget = newTarget;
        EnterState(combat);
    }

    //called via animation state beaviour script after "Attack" is finished
    public void OnApplyAttack()
    {
        if (enemyPawnTarget != null && hp > 0)
        {
            enemyPawnTarget.hp -= attackPower;
            enemyPawnTarget.CheckForDeath();

            if (enemyPawnTarget.hp <= 0)
            {
                enemyPawnTarget = null;
                EnterState(advancing);
            }
        }
    }


    public void CheckForDeath()
    {
        if (defeated) return;

        if (hp <= 0)
        {
            enemyPawnTarget = null;
            EnterState(defeat);
            defeated = true;
        }
    }

    //called via animation state
    public IEnumerator OnDefeated()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        animator.enabled = false;

        StartCoroutine(FieldEvents.LerpValuesCoRo(1, 0, 1, alpha =>
        {
            var color = sr.color;
            color.a = alpha;
            sr.color = color;
        }));

        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }

    private void Update()
    {

       // if (Input.GetKeyDown(KeyCode.Escape))
       // {
       //     Vector3 direction = (targetFortress.transform.position - this.transform.position).normalized;
       //     float attackDirX = Mathf.Sign(direction.x);
       //
       //     Debug.Log(attackDirX);
       //
       // }
    }

    void FixedUpdate()
    {
        state.PawnUpdate();

    }
}