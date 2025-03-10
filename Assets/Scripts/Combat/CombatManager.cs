using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public State currentState;

    [Header("Settings")]

    public Battle battleScheme;

    public GameObject player;
    public Enemy[] enemy;
    public List<Ally> allies;

    [Header("PlayerMoves")]
    public PlayerMove selectedPlayerMove;
    public int selectedEnemy = 0;

    [Header("Debugging")]
    public PlayerCombatStats playerCombatStats;

    public CombatUIManager CombatUIManager;
    public PlayerMoveManager playerMoveManager;
    public int roundCount;

    [Header("States")]
    public Setup setup;
    public FirstMove firstMove;
    public SecondMove secondMove;
    public EnemySelect enemySelect;
    public AttackTarget attackTarget;
    public ApplyMove applyMove;
    public EnemyAttack enemyAttack;
    public RoundReset roundReset;
    public Victory victory;
    public Defeat defeat;
    public GearSelect gearSelect;

    [Header("Movement")]

    public GameObject combatMovementPrefab;
    public CameraFollow cameraFollow;
    public Animator playerAnimator;

    [HideInInspector] public int enemyRawAttackPower;

    private void OnEnable()
    {
        CombatEvents.PlayerDefeated += PlayerDefeated;
    }

    private void OnDisable()
    {
        CombatEvents.PlayerDefeated -= PlayerDefeated;
    }

    public void StartBattle()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMoveManager = player.GetComponentInChildren<PlayerMoveManager>();

        if (playerMoveManager == null)
        {
            Debug.LogError("PlayerMoveManager is not found in StartBattle");
            return;
        }

        enemy = new Enemy[battleScheme.enemyGameObject.Length];

        for (int i = 0; i < battleScheme.enemyGameObject.Length; i++)
        {
            enemy[i] = battleScheme.enemyGameObject[i].GetComponent<Enemy>();
            if (enemy[i] == null)
            {
                Debug.LogError("Enemy component not found on " + battleScheme.enemyGameObject[i].name);
            }
        }

        if (battleScheme.allies != null)
        
        {
            allies = new List<Ally>(battleScheme.allies);
        }

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

    public IEnumerator PositionCombatant(GameObject goToMove, Transform positionToMove)
    {
        var combatantMoveMentInstanceGO = Instantiate(combatMovementPrefab, this.transform);
        var combatantMoveMentInstance = combatantMoveMentInstanceGO.GetComponent<CombatMovement>();

        combatantMoveMentInstanceGO.name = "MoveCombatant" + goToMove.gameObject.name;
        yield return (combatantMoveMentInstance.MoveCombatant(goToMove, positionToMove.position));
        Destroy(combatantMoveMentInstanceGO);
    }

    void PlayerDefeated()
    {
        defeat.playerDefeated = true;
        SetState(defeat);
    }
}