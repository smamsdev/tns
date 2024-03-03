using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public enum BattleState { NoBattle, RoundStartFirstMove = 1, SecondMove = 2, SelectAttackTarget = 3, ApplyMove, Animating, EnemyAttack, Reset, Setup, Victory, Defeat }

public class CombatManagerV3 : MonoBehaviour
{

    BattleState internalbattleState; //cross check if the battlestate was changed via inspector for debugging


    [Header("Settings")]
    public Battle battleScheme;
    public GameObject player;

    [Header("Debugging")]
    public BattleState battleState;
    public CombatUIScript combatUIScript;
    public AttackTargetMenuScript attackTargetMenuScript;
    public PlayerMoveManagerSO playerMoveManager;
    public PlayerStatsSO playerStats;
    public int roundCount;
    public bool enemyIsDead = false;
    GameObject BattleUpdate;

    [Header("States")]
    [SerializeField] Setup setup;
    [SerializeField] FirstMove firstMove;
    [SerializeField] SecondMove secondMove;
    [SerializeField] AttackTarget attackTarget;
    [SerializeField] ApplyMove applyMove;
    [SerializeField] EnemyAttack enemyAttack;
    [SerializeField] RoundReset roundReset;
    [SerializeField] Victory victory;


    [HideInInspector] public Vector2 enemyGameObjectDefaultPosition; //default position data is required as a return point after attacking
    [HideInInspector] public State currentState;
    [HideInInspector] public int enemyRawAttackPower;


    private void OnEnable()
    {
        CombatEvents.EnemyAttackPower += EnemyRawAttackPowerIS;
        CombatEvents.UpdateFighterPosition += UpdateFighterPosition;
    }

    private void OnDisable()
    {
        CombatEvents.EnemyAttackPower -= EnemyRawAttackPowerIS;
        CombatEvents.UpdateFighterPosition -= UpdateFighterPosition;
    }

    private void Start()
    {
        BattleUpdate = GameObject.Find("BattleUpdate");
      //  BattleUpdate.SetActive(false); delete
    }

    public void SetState(State state)

    {
        currentState = state;

    }

    public void SetBattleSetupBattle()

    {
        StartCoroutine(setup.StartState());
        battleState = BattleState.Setup;
        internalbattleState = battleState;
    }

    public void SetBattleStateFirstMove()
 
    {
        StartCoroutine(firstMove.StartState());
        battleState = BattleState.RoundStartFirstMove;
        internalbattleState = battleState;

        BattleUpdate.SetActive(true);
    }

    public void SetBattleStateSecondMove()

    {
        StartCoroutine(secondMove.StartState());

        battleState = BattleState.SecondMove;
        internalbattleState = battleState;

        BattleUpdate.SetActive(true);
    }


    public void SetBattleStateAttackTarget()

    {
        StartCoroutine(attackTarget.StartState());
        battleState = BattleState.SelectAttackTarget;
        internalbattleState = battleState;

        BattleUpdate.SetActive(true);
    }


    public void SetBattleStateApplyPlayerMove()

    {
        StartCoroutine(applyMove.StartState());
        battleState = BattleState.ApplyMove;
        internalbattleState = battleState;

        BattleUpdate.SetActive(true);
    }

    public void SetBattleStateEnemyAttack()

    {
        StartCoroutine(enemyAttack.StartState());
        battleState  = BattleState.EnemyAttack;
        internalbattleState = battleState;

        BattleUpdate.SetActive(true);
    }

    public void SetBattleStateVictory()

    {
        StartCoroutine(victory.StartState());
        battleState = BattleState.Victory;
        internalbattleState = battleState;

        BattleUpdate.SetActive(true);
    }

    void EnemyRawAttackPowerIS(int value)
    { enemyRawAttackPower = value; }

    public void SetBattleStateRoundReset()

    {
        StartCoroutine(roundReset.StartState());
    }

    private void Update()

    {
        if (internalbattleState != battleState)
    
        {
            this.transform.GetChild(2).gameObject.SetActive(false);

            switch (battleState) 
            
            {
                case BattleState.RoundStartFirstMove: SetBattleStateFirstMove(); break;

                case BattleState.SecondMove: SetBattleStateSecondMove(); break;

                case BattleState.SelectAttackTarget: SetBattleStateAttackTarget(); break;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
            
        {  
            playerStats.LevelUp();
        }

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
        fighterGameObject.transform.position = end;
    }

}
