using System.Collections;
using UnityEngine;

public class ShakesPerformance : MonoBehaviour
{
    [SerializeField] PlayerMovementScript playerMovementScript;
    [SerializeField] GameObject stageExploredGO;

    public BooleanCheckTrigger stageAreaExploredCheck;
    public BooleanCheckTrigger jumpsCompleteCheck;
    public BooleanCheckTrigger focusTimerCompleteCheck;

    public Animator playerAnimator;
    public float timer = 0f;
    public int jumpCounter = 0;
    public float jumpCooldown;
    private float idleTimer = 0f;
    private float idleThreshold = 7f;
    public bool isPerformance = true;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isPerformance)
        {
            timer += Time.deltaTime;
            jumpCooldown -= Time.deltaTime;

            if (timer > 20)
            {
                EndPerformance();
            }

            if (playerMovementScript.movementDirection == Vector2.zero)
            {
                idleTimer += Time.deltaTime;

                if (idleTimer >= idleThreshold)
                {
                    focusTimerCompleteCheck.conditionMet = true;
                }


            }

            if (Input.GetKeyDown(KeyCode.Space) && jumpCooldown < 0.1f)
            {
                CombatEvents.LockPlayerMovement();
                playerAnimator.SetBool("isMoving", false);
                idleTimer = 0f;

                if (playerMovementScript.lookDirection.x < 0)
                {
                    playerAnimator.SetTrigger("Trigger2");
                }

                else
                {
                    playerAnimator.SetTrigger("Trigger1");
                }

                jumpCounter++;
                jumpCooldown = 1f;

                if (jumpCounter == 10)
                {
                    jumpsCompleteCheck.conditionMet = true;
                    jumpCounter = 11;
                }

                CombatEvents.UnlockPlayerMovement();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")

        {
            stageAreaExploredCheck.conditionMet = true;
        }

    }

    void EndPerformance()
    {
        isPerformance = false;
        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }

}