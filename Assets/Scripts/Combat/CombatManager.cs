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

    [Header("PlayerMoves")]
    public PlayerMove selectedPlayerMove;
    public int selectedEnemy = 0;

    [Header("Debugging")]
    public PlayerCombatStats playerCombatStats;

    public CombatUIManager CombatUIManager;
    public PlayerMoveManager playerMoveManager;
    public int roundCount;
    public bool enemyIsDead = false;

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
    public GearSelect gearSelect;

    [Header("Movement")]

    public CombatMovement combatMovement;
    public CameraFollow cameraFollow;
    public Animator playerAnimator;

    [HideInInspector] public int enemyRawAttackPower;

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
}