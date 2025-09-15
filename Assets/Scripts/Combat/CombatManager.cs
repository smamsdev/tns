using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CombatManager : MonoBehaviour
{
    [Header("Battle Setup")]
    public Battle battleScheme;
    //public GameObject player;
    public PlayerCombat playerCombat;
    public List<Combatant> allies;
    public List<Combatant> enemies;
    public List<Combatant> allAlliesToTarget;

    [Header("Debugging")]
    public State currentState;

    [Header("States")]
    public Setup setup;
    public FirstMoveState firstMove;
    public SecondMoveState secondMove;
    public EnemySelectState enemySelectState;
    public AllyMoveState allyMoveState;
    public ApplyPlayerMove applyMove;
    public EnemyMoveState enemyMoveState;
    public RoundReset roundReset;
    public VictoryState victory;
    public Defeat defeat;
    public TacticalSelectState tacticalSelectState;
    public EquipSlotSelectState equipSlotSelectState;
    public GearSelectCombatState gearSelectCombatState;

    [Header("Misc")]
    public GameObject combatMovementPrefab;
    public CameraFollow cameraFollow;
    public CombatMenuManager combatMenuManager;
    public int roundCount;
    public Combatant lastCombatantTargeted;

    private void OnEnable()
    {
        CombatEvents.PlayerDefeated += PlayerDefeated;
        cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
    }

    private void OnDisable()
    {
        CombatEvents.PlayerDefeated -= PlayerDefeated;
    }

    private void Start()
    {
        InitializePermanentStatsAndGear();
    }

    void InitializePermanentStatsAndGear()
    {
        playerCombat.playerInventory.InstantiateAllEquippedGear(this);

        playerCombat.maxHP = playerCombat.playerPermanentStats.maxHP;
        playerCombat.CurrentHP = playerCombat.playerPermanentStats.currentHP;
        playerCombat.currentPotential = playerCombat.playerPermanentStats.currentPotential;
        playerCombat.attackBase = playerCombat.playerPermanentStats.attackBase;
        playerCombat.fendBase = playerCombat.playerPermanentStats.fendBase;
        playerCombat.focusBase = playerCombat.playerPermanentStats.focusBase;

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

        //SetState(victory);
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

        yield return (combatantMoveMentInstance.MoveCombatant(goToMove, targetPosition, stoppingPercentage, useTimeout = false));
        Destroy(combatantMoveMentInstanceGO);
    }

    public void SelectCombatantMove(Combatant combatant)
    {
        combatant.SelectMove();
        combatant.moveSelected.LoadMoveStatsAndPassCBM(combatant, this);
    }

    void PlayerDefeated()
    {
        defeat.playerDefeated = true;
        SetState(defeat);
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
        combatant.movementScript.rigidBody2d.bodyType = bodyType;
    }
}