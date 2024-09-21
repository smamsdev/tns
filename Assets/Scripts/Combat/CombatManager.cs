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

    [Header("PlayerMove")]
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

    public MoveTo moveTo;

    [HideInInspector] public int enemyRawAttackPower;

    public void StartBattle()
    {
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

    public void UpdateFighterPosition(GameObject fighterGameObject, Vector2 end, float seconds)
    {
        if (fighterGameObject == null)
        {
            Debug.LogError("Fighter GameObject is null in UpdateFighterPosition");
            return;
        }

        StartCoroutine(UpdateEnemyPositionCoRoutine(fighterGameObject, end, seconds));
    }

    public IEnumerator UpdateEnemyPositionCoRoutine(GameObject fighterGameObject, Vector2 end, float seconds)
    {
        float elapsedTime = 0;
        Vector2 startingPos = fighterGameObject.transform.position;

        while (elapsedTime < seconds)
        {
            fighterGameObject.transform.position = Vector2.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        fighterGameObject.transform.position = end; // Ensure final position is set
    }
}