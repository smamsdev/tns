using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatModifier;
using static UnityEngine.Rendering.DebugUI;

public abstract class Combatant : MonoBehaviour
{
    public string combatantName;
    public CombatantUI combatantUI;
    public MovementScript movementScript;
    public GameObject fightingPosition;
    public bool isBackstabbed;
    public bool isEnclosed;
    public Collider2D collisionCollider;

    [Header("Moves")]
    public Combatant targetCombatant;
    [SerializeField] MoveSO moveSOSelected;
    public GameObject currentMoveBehaviourParent;
    public MoveBehaviour currentMoveBehaviour;
    public List<MoveSO> moveList = new();

    public int CombatLookDirX
    {
        get => combatLookDirX;
        set
        {
            combatLookDirX = value;
            movementScript.animator.SetFloat("CombatLookDirX", value);
        }
    }

    [SerializeField] private int combatLookDirX;

    [Header("Stats")]
    [SerializeField] private int _attackBase;
    public int AttackBase
    {
        get => (_attackBase);
        set => _attackBase = Mathf.Clamp(value, 0, 999);
    }

    [SerializeField] private int _fendBase;
    public int FendBase
    {
        get => (_fendBase);
        set
        {
            _fendBase = Mathf.Clamp(value, 0, 999);
            //Debug.Log("fend changed");
        }

    }

    [SerializeField] private int _maxHP;
    public int MaxHP
    {
        get => (_maxHP);
        set => _maxHP = Mathf.Clamp(value, 0, 9999);
    }

    [SerializeField] private int _currentHP;
    public int CurrentHP
    {
        get => _currentHP;
        set => _currentHP = Mathf.Clamp(value, 0, MaxHP);
    }


    [Header("Written by MoveBehaviour")]
    [SerializeField] int _attackTotal = 0;
    public int AttackTotal
    { 
        get => _attackTotal;
        set => _attackTotal = Mathf.Clamp(value, 0, 9999);
    }

    [Header ("Written by MoveBehaviour")]
    [SerializeField] int _fendTotal = 0;
    public int FendTotal
    {
        get => _fendTotal;
        set => _fendTotal = Mathf.Clamp(value, 0, 9999);
    }

    private void OnEnable()
    {
        movementScript = GetComponent<MovementScript>();
    }

    public void InstantiateMoveBehaviour(MoveSO moveSO)
    {
        for (int i = currentMoveBehaviourParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(currentMoveBehaviourParent.transform.GetChild(i).gameObject);
        }

        GameObject moveBehaviourGO = Instantiate(moveSO.MovePrefab, currentMoveBehaviourParent.transform);

        moveSOSelected = moveSO;
        moveBehaviourGO.name = moveSO.name + "Behaviour";
        currentMoveBehaviour = moveBehaviourGO.GetComponent<MoveBehaviour>();
    }

    public virtual void SelectMove(CombatManager combatManager)
    {
        int MoveWeightingTotal = 0;

        foreach (MoveSO moveSO in moveList)
        {
            if (moveSO.MoveWeighting > 0)
            {
                MoveWeightingTotal += moveSO.MoveWeighting;
            }
        }

        if (MoveWeightingTotal == 0)
        {
            Debug.LogError("No valid moves available to select!!");
            return;
        }

        int randomValue = Random.Range(1, MoveWeightingTotal + 1);

        foreach (MoveSO moveSO in moveList)
        {
            if (moveSO.MoveWeighting == 0)
                continue;

            if (randomValue > moveSO.MoveWeighting)
                randomValue -= moveSO.MoveWeighting;

            else
            {
                InstantiateMoveBehaviour(moveSO);
                return;
            }
        }

        Debug.LogError("Failed to select a move! This should never happen. Random value was " + randomValue);
    }

    public virtual IEnumerator CombatUpdateHPCoRo(int change)
    {
        combatantUI.statsDisplay.ShowStatsDisplay(true);
        combatantUI.statsDisplay.ShowCombatantName(false);

        if (change >= 0)
            combatantUI.statsDisplay.HPTMPAnimator.Play("CombatUIStatPlus");

        else
            combatantUI.statsDisplay.HPTMPAnimator.Play("CombatUIStatMinus");

        int initialHP = CurrentHP;
        int finalHP = Mathf.Clamp(CurrentHP + change, 0, 9999); 
        float lerpDuration = .5f;

        yield return FieldEvents.LerpValuesCoRo(initialHP, finalHP, lerpDuration, (output) => 
        {
            int outputInt = Mathf.RoundToInt(output);

            CurrentHP = outputInt;
            combatantUI.statsDisplay.UpdateHPDisplay(CurrentHP);

        });

        CurrentHP = finalHP;
        combatantUI.statsDisplay.UpdateHPDisplay(finalHP);

        if (CurrentHP == 0)
        {
            combatantUI.statsDisplay.statsDisplayContainerAnimator.Play("StatsDisplayOnDefeat");

            movementScript.animator.Play("Fall");
            yield return new WaitForSeconds(1f);
            yield break;
        }

        yield return new WaitForSeconds(.5f);
        combatantUI.statsDisplay.statsDisplayContainerAnimator.Play("CombatUIStatsFade");
        yield return new WaitForSeconds(.5f);
        combatantUI.statsDisplay.ShowStatsDisplay(false);
    }

    public IEnumerator CombatHPChanged(int attackRemainder)
    {
        StartCoroutine(combatantUI.damageTakenDisplay.ShowDamageDisplayCoro(attackRemainder));
        yield return CombatUpdateHPCoRo(-attackRemainder);
        combatantUI.statsDisplay.ShowStatsDisplay(false);
    }

    public virtual void ChangeStat(StatModifier mod)
    {
        int baseValueToChange;

        switch (mod.statToChange)
        {
            case StatToChange.AttackBase:
                baseValueToChange = AttackBase;
                break;
            case StatToChange.FendBase:
                baseValueToChange = FendBase;
                break;
            case StatToChange.MaxHP:
                baseValueToChange = MaxHP;
                break;
        //
        //  //PartyMemberStats
        //  case StatToChange.XP:
        //      baseValueToChange = Xp;
        //      break;
        //
        //  // Player stats
        //  case StatToChange.Smams:
        //      baseValueToChange = value;
        //      break;
        //  case StatToChange.MaxPotential:
        //      baseValueToChange = value;
        //      break;
        //  case StatToChange.FocusBase:
        //      baseValueToChange = value;
        //      break;
        //
            default:
                break;
        }
    }
}
