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
        this.transform.GetChild(2).gameObject.SetActive(false);
        player = GameObject.Find("Player");
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
