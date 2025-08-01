using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CombatManager : MonoBehaviour
{
    [Header("Battle Setup")]
    public Battle battleScheme;
    public GameObject player;
    public PlayerCombat playerCombat;
    public List<Combatant> allies;
    public List<Combatant> enemies;
    public List<Combatant> allAlliesToTarget;

    [Header("Debugging")]
    public State currentState;

    [Header("States")]
    public Setup setup;
    public FirstMove firstMove;
    public SecondMove secondMove;
    public EnemySelect enemySelect;
    public AllyMoveState allyMoveState;
    public ApplyPlayerMove applyMove;
    public EnemyMoveState enemyMoveState;
    public RoundReset roundReset;
    public Victory victory;
    public Defeat defeat;
    public GearSelectState gearSelectState;

    [Header("Misc")]
    public GameObject combatMovementPrefab;
    public CameraFollow cameraFollow;
    public CombatMenuManager combatMenuManager;
    public int roundCount;
    public Combatant lastCombatantTargeted;

    private void OnEnable()
    {
        CombatEvents.PlayerDefeated += PlayerDefeated;
    }

    private void OnDisable()
    {
        CombatEvents.PlayerDefeated -= PlayerDefeated;
    }

    public void CombatantDefeated(Combatant defeatedCombatant)
    {
        if (defeatedCombatant is Enemy)
        {
            enemies.Remove(defeatedCombatant);
            defeatedCombatant.combatantUI.statsDisplay.ShowStatsDisplay(false);

            foreach (Ally ally in allies)
            {
                if (enemies.Count > 0 && ally.targetToAttack == defeatedCombatant)
                {
                    SelectTargetToAttack(ally, enemies);
                }
            }
        }

        if (defeatedCombatant is Ally)
        {
            allies.Remove(defeatedCombatant);
            allAlliesToTarget.Remove(defeatedCombatant);
            defeatedCombatant.combatantUI.statsDisplay.ShowStatsDisplay(false);

            foreach (Enemy enemy in enemies)
            {
                if (enemy.targetToAttack == defeatedCombatant)
                {
                    SelectTargetToAttack(enemy, allAlliesToTarget);
                }
            }
        }
    }

    public void StartBattle()
    {
        CombatEvents.LockPlayerMovement.Invoke();

        player = GameObject.FindGameObjectWithTag("Player");
        playerCombat = player.GetComponentInChildren<PlayerCombat>();

        if (playerCombat.playerMoveManager == null)
        {
            Debug.LogError("PlayerMoveManager is not found in StartBattle");
            return;
        }

        if (battleScheme.allies != null)
        
        {
            allies = new List<Combatant>(battleScheme.allies);
        }

        if (battleScheme.enemies != null)

        {
            enemies = new List<Combatant>(battleScheme.enemies);
        }

        else { Debug.Log("enemy not set in Battle"); }

        allAlliesToTarget = new List<Combatant> { playerCombat }; //create a new list and add player do it, this is the pool of allies enemies can attack
        allAlliesToTarget.AddRange(allies);

        SetState(setup);
    }

    public void SetState(State state)
    {
        currentState = state;
        StartCoroutine(currentState.StartState());
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.StateUpdate();
        }
    }

    public IEnumerator PositionCombatant(GameObject goToMove, Vector3 targetPosition, float stoppingPercentage = 100f, bool useTimeout = false)
    {
        var combatantMoveMentInstanceGO = Instantiate(combatMovementPrefab, this.transform);
        var combatantMoveMentInstance = combatantMoveMentInstanceGO.GetComponent<CombatMovement>();
        combatantMoveMentInstanceGO.name = "MoveCombatant" + goToMove.gameObject.name;

        yield return (combatantMoveMentInstance.MoveCombatant(goToMove, targetPosition, stoppingPercentage = 100f, useTimeout = false));
        Destroy(combatantMoveMentInstanceGO);
    }

    public void SelectAndDisplayCombatantMove(Combatant combatant)
    {
        combatant.SelectMove();
        combatant.moveSelected.LoadMoveStats(combatant, this);
        combatant.combatantUI.attackDisplay.UpdateAttackDisplay(combatant.attackTotal);
        combatant.combatantUI.fendScript.fendTextMeshProUGUI.text = combatant.fendTotal.ToString();

        if (combatant.attackTotal > 0)
        {
            combatant.combatantUI.attackDisplay.ShowAttackDisplay(true);
        }

        combatant.combatantUI.fendScript.ShowFendDisplay(combatant, true);
    }

    void PlayerDefeated()
    {
        defeat.playerDefeated = true;
        SetState(defeat);
    }

    public void SetUIBasedOnLookDir(Combatant combatant)
    {
        var flippedPos = combatant.combatantUI.attackDisplay.transform.localPosition;
        flippedPos.x = -flippedPos.x;
        combatant.combatantUI.attackDisplay.transform.localPosition = flippedPos;
    }

    public void SelectTargetToAttack(Combatant combatant, List<Combatant> targetList)
    {
        combatant.targetToAttack = targetList[Random.Range(0, targetList.Count)];

        Vector3 direction = (combatant.targetToAttack.transform.position - combatant.transform.position).normalized;
        float attackDirX = Mathf.Sign(direction.x);

        combatant.GetComponent<MovementScript>().lookDirection = new Vector2(attackDirX, 0);
    }

    public void SetRigidBodyType(Combatant combatant, RigidbodyType2D bodyType)
    {
        combatant.GetComponent<ActorMovementScript>().actorRigidBody2d.bodyType = bodyType;
    }
}