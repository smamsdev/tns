using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum BattleState { NoBattle, RoundStartFirstMove, SecondMove, SelectAttackTarget, ApplyMove, Animating, EnemyAttack, Reset, Setup, Victory, Defeat }

public class CombatManagerV2 : MonoBehaviour
{

    public BattleState battleState;

    public bool playerMoveCompleted;
    public bool enemyAttackCompleted;

    public int enemyRawAttackPower;

    int roundCounterName;
    TextMeshPro roundCounterText;
    [SerializeField] GameObject roundCounter;

    [SerializeField] GameObject combatMenuContainerObj;
    [SerializeField] GameObject playerStatsObj;

    [SerializeField] CombatUIScript combatUIScript;
    [SerializeField] PlayerMovementScript playerMovementScript;

    [SerializeField] AttackTargetMenuScript attackTargetMenuScript;

    [SerializeField] PlayerMoveManagerSO playerMoveManager;
    [SerializeField] PlayerStatsSO playerStats;

    [SerializeField] GameObject enemyLoaderObject;
    [SerializeField] GameObject playerDefaultPosition;

    private void OnEnable()
    {
        CombatEvents.EnemyIsDefeated += Victory;
        CombatEvents.EnemyAttackPower += EnemyRawAttackPowerIS;
        CombatEvents.BeginBattle += Setup;
    }

    private void OnDisable()
    {
        CombatEvents.EnemyIsDefeated -= Victory;
        CombatEvents.EnemyAttackPower -= EnemyRawAttackPowerIS;
        CombatEvents.BeginBattle -= Setup;
    }

    private void Start()
    {
        SetBattleStateNoBattle();
    }

    void RoundStart()

    {
        combatUIScript.firstdMoveIsBeingDecided = false;
        combatUIScript.secondAttackButtonIsHighlighted = false;

        combatMenuContainerObj.SetActive(true);
        combatUIScript.ShowFirstMoveMenu();
        playerMoveManager.firstMoveIs = 0;

        CombatEvents.ShowHideFendDisplay?.Invoke(false);
        CombatEvents.GetEnemyAttackPower?.Invoke();
    }

    void SetSecondMove()

    {
        if (Input.GetKeyDown(KeyCode.Escape)) { SetBattleStateRoundStart(); }

        combatUIScript.firstAttackButtonIsHighlighted = false;  
        combatUIScript.secondMoveIsBeingDecided = false;
        combatUIScript.ShowSecondMoveMenu();
        playerMoveManager.secondMoveIs = 0;
    }


    void SetAttackTarget()

    {
        
        if (Input.GetKeyDown(KeyCode.Escape)) { SetBattleStateSecondMove(); }
        
        
        combatUIScript.secondAttackButtonIsHighlighted = false;
        if (playerMoveManager.firstMoveIs == 1 || playerMoveManager.secondMoveIs == 1)

        {
            attackTargetMenuScript.DisplayAttackTargetMenuCoRoutine();

        }
        else
        {
            SetBattleStateApplyMove();
        }
    }


    IEnumerator ApplyMove()

    {
        if (playerMoveCompleted == false)
        {
            playerMoveCompleted = true;
            yield return new WaitForSeconds(0.6f);

            CombatEvents.ShowHideFendDisplay.Invoke(true);
            combatUIScript.HideTargetMenu();
            combatMenuContainerObj.SetActive(false);
            CombatEvents.HighlightBodypartTarget.Invoke(false, false, false);


            CombatEvents.UpdateNarrator.Invoke(playerMoveManager.moveForNarrator);

            if (playerMoveManager.firstMoveIs ==1 || playerMoveManager.secondMoveIs == 1)
            {
                CombatEvents.UpdatePlayerPosition.Invoke(new Vector2(enemyLoaderObject.transform.position.x - 0.3f, enemyLoaderObject.transform.position.y), 0.5f);
                yield return new WaitForSeconds(1);
            }

            if (playerMoveManager.firstMoveIs == 1 || playerMoveManager.secondMoveIs == 1)
            {
                CombatEvents.UpdatePlayerPosition.Invoke(playerDefaultPosition.transform.position, 0.5f);
            }

            CombatEvents.CalculateEnemyDamageTaken.Invoke(playerStats.attackPower);

            CombatEvents.IsEnemyDefeated.Invoke();

            if (battleState != BattleState.Victory) 
            { 
            
                battleState = BattleState.EnemyAttack;
            }

        }
    }

    IEnumerator EnemyAttack()

    {
        if (enemyAttackCompleted == false)
        {
            enemyAttackCompleted = true;


            yield return new WaitForSeconds(1);

            int enemyAttackPower = Mathf.Clamp(enemyRawAttackPower - playerStats.playerFend, 0, 9999);
            CombatEvents.UpdatePlayerHP.Invoke(enemyAttackPower);

            StartCoroutine (AnimateEnemyCoRoutine());

            SetBattleStateReset();
        }
    }

    IEnumerator AnimateEnemyCoRoutine() 
    
    {

        CombatEvents.UpdateEnemyPosition.Invoke(new Vector2 (playerDefaultPosition.transform.position.x+0.3f, playerDefaultPosition.transform.position.y), 0.5f);
        yield return new WaitForSeconds(0.5f);

        CombatEvents.UpdateFendDisplay.Invoke(playerStats.playerFend - enemyRawAttackPower);

        yield return new WaitForSeconds(0.5f);

        CombatEvents.UpdateEnemyPosition.Invoke(enemyLoaderObject.transform.position, 0.5f);
    }

    void EnemyRawAttackPowerIS(int value)
    { enemyRawAttackPower = value; }

    IEnumerator RoundReset()

    {

        yield return new WaitForSeconds(1f);

        playerMoveCompleted = false;
        enemyAttackCompleted = false;


        combatUIScript.firstdMoveIsBeingDecided = false;
        combatUIScript.secondMoveIsBeingDecided = false;


        combatUIScript.HideSecondMenu();
        combatUIScript.HideTargetMenu();

        playerStats.ResetAllMoveMods();

        attackTargetMenuScript.attackTargetMenu.SetActive(false);
        attackTargetMenuScript.targetSelected = false;
        attackTargetMenuScript.targetIsSet = 0;

        roundCounterName++;
        roundCounterText.text = "CURRENT ROUND: " + roundCounterName.ToString();

        CombatEvents.UpdateFendDisplay.Invoke(0);
        CombatEvents.UpdateNarrator.Invoke("");

        playerMoveManager.firstMoveIs = 0;
        playerMoveManager.secondMoveIs = 0;


        if (battleState != BattleState.Victory)
        {

            SetBattleStateRoundStart();
        }



    }

    void Victory()

    {
        StartCoroutine(RoundReset());
        battleState = BattleState.Victory;
        playerStatsObj.SetActive(false);
        playerMovementScript.movementLocked = false;
    }

    void Setup()

    {
        playerMovementScript.movementLocked = true;
        playerMovementScript.playerPosition = new Vector2(-1.7f, -0.4f);
        roundCounterText = roundCounter.GetComponent<TextMeshPro>();
        playerStatsObj.SetActive(true);
        playerStats.InitalisePlayerStats();
        CombatEvents.InitializePlayerHP.Invoke(playerStats.playerMaxHP);

        battleState = BattleState.RoundStartFirstMove;
        enemyRawAttackPower = 0;
    }


    void Update()
    {
        switch (battleState)
        { 
            case BattleState.RoundStartFirstMove: RoundStart(); break;
            case BattleState.SecondMove: SetSecondMove(); break;
            case BattleState.SelectAttackTarget: SetAttackTarget(); break;
            case BattleState.ApplyMove: StartCoroutine(ApplyMove()); break;
            case BattleState.EnemyAttack: StartCoroutine(EnemyAttack()); break;
            case BattleState.Reset: StartCoroutine(RoundReset()); break;
        }
    }

    //BattleState setter functions

    public void SetBattleStateNoBattle()
    { battleState = BattleState.NoBattle; }

    public void SetBattleStateSetup()
    { battleState = BattleState.Setup; }

    public void SetBattleStateRoundStart()
    { battleState = BattleState.RoundStartFirstMove; }

    public void SetBattleStateFirstMove()
    { battleState = BattleState.RoundStartFirstMove; }

    public void SetBattleStateSecondMove()
    { battleState = BattleState.SecondMove; }

    public void SetBattleStateAttackTarget()
    { battleState = BattleState.SelectAttackTarget; }

    public void SetBattleStateApplyMove()
    { battleState = BattleState.ApplyMove; }

    public void SetBattleStateAnimating()
    { battleState = BattleState.Animating; }

    public void SetBattleStateEnemyAttack()
    { battleState = BattleState.EnemyAttack; }

    public void SetBattleStateReset()
    { battleState = BattleState.Reset; }

}
