using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Battle Setup")]
    public Battle battleScheme;
    public GameObject player;
    public PlayerCombat playerCombat;
    public List<Ally> allies;
    public List<Enemy> enemies;
    public List<Combatant> allAllies;

    [Header("Debugging")]
    public State currentState;

    [Header("States")]
    public Setup setup;
    public FirstMove firstMove;
    public SecondMove secondMove;
    public EnemySelect enemySelect;
    public AttackTarget attackTarget;
    public AllyMoveState allyMoveState;
    public ApplyPlayerMove applyMove;
    public EnemyMoveState enemyMoveState;
    public RoundReset roundReset;
    public Victory victory;
    public Defeat defeat;
    public GearSelect gearSelect;

    [Header("Misc")]
    public GameObject combatMovementPrefab;
    public CameraFollow cameraFollow;
    public CombatMenuManager combatMenuManager;
    public int roundCount;
    public Combatant lastCombatantTargeted;

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
        CombatEvents.LockPlayerMovement.Invoke();

        player = GameObject.FindGameObjectWithTag("Player");
        playerCombat = player.GetComponentInChildren<PlayerCombat>();

        if (playerCombat.playerMoveManager == null)
        {
            Debug.LogError("PlayerMoveManager is not found in StartBattle");
            return;
        }

        if (battleScheme.allies != null)
        
        {
            allies = new List<Ally>(battleScheme.allies);
        }

        if (battleScheme.enemies != null)

        {
            enemies = new List<Enemy>(battleScheme.enemies);
        }

        else { Debug.Log("enemy not set in Battle"); }

        allAllies = new List<Combatant> { playerCombat };
        allAllies.AddRange(allies);

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

        yield return (combatantMoveMentInstance.MoveCombatant(goToMove, targetPosition, stoppingPercentage = 100f, useTimeout = false));
        Destroy(combatantMoveMentInstanceGO);
    }

    public void SelectAndDisplayCombatantMove(Combatant combatant)
    {
        combatant.SelectMove();
        combatant.moveSelected.LoadMoveStats(combatant, this);
        combatant.combatantUI.attackDisplay.UpdateAttackDisplay(combatant.attackTotal);
        combatant.combatantUI.fendScript.fendTextMeshProUGUI.text = combatant.fendTotal.ToString();

        if (combatant.attackTotal > 0)
        {
            combatant.combatantUI.attackDisplay.ShowAttackDisplay(true);
        }

            combatant.combatantUI.fendScript.ShowFendDisplay(combatant, true);
    }

    void PlayerDefeated()
    {
        defeat.playerDefeated = true;
        SetState(defeat);
    }
}