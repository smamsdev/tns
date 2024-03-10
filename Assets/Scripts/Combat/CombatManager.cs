using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public State currentState;

    [Header("Settings")]
    public Battle battleScheme;
    public GameObject player;
    public Enemy enemy;

    [Header("Debugging")]
    public CombatUIScript combatUIScript;
    public AttackTargetMenuScript attackTargetMenuScript;
    public PlayerMoveManagerSO playerMoveManager;
    public PlayerStatsSO playerStats;
    public int roundCount;
    public bool enemyIsDead = false;


    [Header("States")]
    public Setup setup;
    public FirstMove firstMove;
    public SecondMove secondMove;
    public AttackTarget attackTarget;
    public ApplyMove applyMove;
    public EnemyAttack enemyAttack;
    public RoundReset roundReset;
    public Victory victory;
    public GearSelect gearSelect;

    [HideInInspector] public Vector2 enemyGameObjectDefaultPosition; //default position data is required as a return point after attacking
    [HideInInspector] public int enemyRawAttackPower;

    private void OnEnable()
    {
        CombatEvents.PassState += SetState;
    }

    private void OnDisable()
    {
        CombatEvents.PassState -= SetState;
    }

    public void StartBattle()
    {
        enemy = battleScheme.enemyGameObject.GetComponent<Enemy>();
        SetState(setup);
    }

    public void SetState(State state)

    {
        currentState = state;
        StartCoroutine(currentState.StartState());
    }

    private void Update()

    {
        currentState.StateUpdate();
    }

    public void UpdateFighterPosition(GameObject fighterGameObject, Vector2 end, float seconds) //call the coroutine using a function because you can't call coroutines when invoking events

    {
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
    }

}
