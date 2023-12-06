using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public enum BattleState { NoBattle, RoundStartFirstMove = 1, SecondMove = 2, SelectAttackTarget = 3, ApplyMove, Animating, EnemyAttack, Reset, Setup, Victory, Defeat }

public class CombatManagerV3 : MonoBehaviour
{

    BattleState internalbattleState; //cross check if the battlestate was changed via inspector for debugging
    public BattleState battleState;

    [HideInInspector] public State currentState;

    [HideInInspector] public int enemyRawAttackPower;

    public CombatUIScript combatUIScript;
    public AttackTargetMenuScript attackTargetMenuScript;
    public PlayerMoveManagerSO playerMoveManager;
    public PlayerStatsSO playerStats;
    public GameObject playerFightingPosition;
    public GameObject enemyGameObject;
    [HideInInspector] public Vector2 enemyGameObjectDefaulPosition; //default position data is required as a return point after attacking
    public int roundCount;
    public bool enemyIsDead = false;

    private void OnEnable()
    {
        CombatEvents.EnemyAttackPower += EnemyRawAttackPowerIS;
    }

    private void OnDisable()
    {
        CombatEvents.EnemyAttackPower -= EnemyRawAttackPowerIS;
    }

    private void Start()
    {
        //SetBattleSetupBattle();
        enemyGameObject.transform.GetChild(0).gameObject.SetActive(true);
        this.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void SetState(State state)

    {
        currentState = state;
        StartCoroutine(currentState.Start());
    }

    public void SetBattleSetupBattle()

    {
        SetState(new Setup(this));
        battleState = BattleState.Setup;
        internalbattleState = battleState;
    }

    public void SetBattleStateFirstMove()

  
    {
        SetState(new FirstMove(this));
        battleState = BattleState.RoundStartFirstMove;
        internalbattleState = battleState;
       
        this.transform.GetChild(2).gameObject.SetActive(true);
    }

    public void SetBattleStateSecondMove()

    {
        SetState(new SecondMove(this));
        battleState = BattleState.SecondMove;
        internalbattleState = battleState;
        
        this.transform.GetChild(2).gameObject.SetActive(true);
    }


    public void SetBattleStateAttackTarget()

    {
        SetState(new AttackTarget(this));

        battleState = BattleState.SelectAttackTarget;
        internalbattleState = battleState;
       
        this.transform.GetChild(2).gameObject.SetActive(true);
    }


    public void SetBattleStateApplyPlayerMove()

    {
        SetState(new ApplyMove(this));
        battleState = BattleState.ApplyMove;
        internalbattleState = battleState;

        this.transform.GetChild(2).gameObject.SetActive(true);
    }

    public void SetBattleStateEnemyAttack()

    {
        SetState(new EnemyAttack(this));
        battleState  = BattleState.EnemyAttack;
        internalbattleState = battleState;

        this.transform.GetChild(2).gameObject.SetActive(true);
    }

    public void SetBattleStateVictory()

    {
        SetState(new Victory(this));
        battleState = BattleState.Victory;
        internalbattleState = battleState;
        this.transform.GetChild(2).gameObject.SetActive(true);
    }



    void EnemyRawAttackPowerIS(int value)
    { enemyRawAttackPower = value; }

    public void SetBattleStateRoundReset()

    {
        SetState(new RoundReset(this));
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

    public void UpdateEnemyPosition(Vector2 end, float seconds) //call the coroutine using a function because you can't call coroutines when invoking events

    {
        StartCoroutine(UpdateEnemyPositionCoRoutine(end, seconds));
    }

    public IEnumerator UpdateEnemyPositionCoRoutine(Vector2 end, float seconds)
    {
        float elapsedTime = 0;
        Vector2 startingPos = enemyGameObject.transform.position;
        while (elapsedTime < seconds)
        {
            enemyGameObject.transform.position = Vector2.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        enemyGameObject.transform.position = end;
    }

}
