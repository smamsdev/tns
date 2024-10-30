using System.Collections;
using UnityEngine;

public class ShakesPerformance : MonoBehaviour
{
    [SerializeField] PlayerMovementScript playerMovementScript;
    [SerializeField] GameObject stageExploredGO;

    public bool focusComplete;
    public bool jumpsComplete = false;
    public BooleanCheckTrigger stageAreaExplored;

    public Animator playerAnimator;
    public float timer = 0f;
    public int jumpCounter = 0;
    public float jumpCooldown;
    private float idleTimer = 0f;
    private float idleThreshold = 7f;
    public bool isPerformance = true;

    private void OnEnable()
    {
        FieldEvents.HasCompleted += TriggerAction;
    }

    private void OnDisable()
    {
        FieldEvents.HasCompleted -= TriggerAction;
    }

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
                    focusComplete = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && jumpCooldown < 0.1f)
            {
                CombatEvents.LockPlayerMovement();
                playerAnimator.SetBool("isMoving", false);

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
                    jumpsComplete = true;
                    jumpCounter = 11;
                }

                CombatEvents.UnlockPlayerMovement();
            }
        }
    }

    void TriggerAction(GameObject gameObject)
    {
        if (stageExploredGO == gameObject)

        {
            stageAreaExplored.conditionMet = true;
        }
    }

    void EndPerformance()
    {
        isPerformance = false;
        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }
}