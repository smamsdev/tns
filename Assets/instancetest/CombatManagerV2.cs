using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum BattleState { RoundStartFirstMove, SecondMove, SelectAttackTarget, ApplyMove, Animating, EnemyAttack, Reset, Setup, Victory, Defeat }

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
    [SerializeField] CombatUIScript combatUIScript;
    [SerializeField] AttackTargetMenuScript attackTargetMenuScript;

    [SerializeField] PlayerMoveManagerSO playerMoveManager;
    [SerializeField] PlayerStatsSO playerStats;

    private void OnEnable()
    {
        CombatEvents.EnemyIsDefeated += SetBattleStateVictory;
        CombatEvents.EnemyAttackPower += EnemyRawAttackPower;
    }

    private void OnDisable()
    {
        CombatEvents.EnemyIsDefeated -= SetBattleStateVictory;
        CombatEvents.EnemyAttackPower -= EnemyRawAttackPower;
    }


//start
    void Start()
    {
        Setup();
        roundCounterText = roundCounter.GetComponent<TextMeshPro>();
    }

    void RoundStart()

    {
        combatMenuContainerObj.SetActive(true);
        combatUIScript.ShowFirstMoveMenu();
        CombatEvents.ShowHideFendDisplay.Invoke(false);

    }

    void SetSecondMove()

    { combatUIScript.ShowSecondMoveMenu(); }


    void SetAttackTarget()

    {
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

            CombatEvents.ShowHideFendDisplay.Invoke(true);
            combatUIScript.HideTargetMenu();
            combatMenuContainerObj.SetActive(false);
            CombatEvents.HighlightBodypartTarget.Invoke(false, false, false);


            CombatEvents.UpdateNarrator.Invoke(playerMoveManager.moveForNarrator);

            yield return new WaitForSeconds(0.3f);

            CombatEvents.UpdateEnemyHP.Invoke(playerStats.attackPower);


            CombatEvents.IsEnemyDefeated.Invoke();

            battleState = BattleState.EnemyAttack;

        }
    }

    IEnumerator EnemyAttack()

    {
        if (enemyAttackCompleted == false)
        {
            enemyAttackCompleted = true;
            CombatEvents.GetEnemyAttackPower.Invoke();
            CombatEvents.UpdateFendDisplay.Invoke(playerStats.playerFend - enemyRawAttackPower);
            yield return new WaitForSeconds(0.3f);



            int enemyAttackPower = Mathf.Clamp(enemyRawAttackPower - playerStats.playerFend, 0, 9999);
            CombatEvents.UpdatePlayerHP.Invoke(enemyAttackPower);


            // Debug.Log("enemy attacks for " + enemyRawAttackPower + " but is blocked for " + playerStats.playerFend + " total damage is " + enemyAttackPower);

            SetBattleStateReset();
        }
    }

    void EnemyRawAttackPower(int value)
    { enemyRawAttackPower = value; }

    void RoundReset()

    {
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

        SetBattleStateRoundStart();
    }

    void Victory()

    {
        Debug.Log("congrats u are gay");
    }

    void Setup()

    {
        playerStats.InitalisePlayerStats();
        CombatEvents.InitializePlayerHP.Invoke(playerStats.playerMaxHP);
        CombatEvents.InitializePartsHP.Invoke();
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
            case BattleState.Reset: RoundReset(); break;
            case BattleState.Victory: Victory(); break;
        }
    }

    //BattleState setter functions
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

    public void SetBattleStateVictory()
    { battleState = BattleState.Victory; }
}
