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

    [Header("Moves")]
    public Combatant targetCombatant;
    public MoveSO moveSOSelected;
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
    [SerializeField] private int attackBase;
    public int AttackBase
    {
        get => (attackBase);
        set => attackBase = Mathf.Clamp(value, 0, 999);
    }

    [SerializeField] private int fendBase;
    public int FendBase
    {
        get => (fendBase);
        set => fendBase = Mathf.Clamp(value, 0, 999);
    }

    [SerializeField] private int maxHP;
    public int MaxHP
    {
        get => (maxHP);
        set => maxHP = Mathf.Clamp(value, 0, 9999);
    }

    [SerializeField] private int currentHP;
    public int CurrentHP
    {
        get => currentHP;
        set => currentHP = Mathf.Clamp(value, 0, 9999);
    }


    [Tooltip("Set by code. Leave as 0.")]
    private int attackTotal = 0;
    public int AttackTotal
    { 
        get => attackTotal;
        set => attackTotal = Mathf.Clamp(value, 0, 9999);
    }

    [Tooltip("Set by code. Leave as 0.")]
    private int fendTotal = 0;
    public int FendTotal
    {
        get => fendTotal;
        set => fendTotal = Mathf.Clamp(value, 0, 9999);
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
                moveSOSelected = moveSO;
                InstantiateMoveBehaviour(moveSO);
                return;
            }
        }

        Debug.LogError("Failed to select a move! This should never happen. Random value was " + randomValue);
    }

    public virtual void UpdateHP(int value)
    {
        StartCoroutine(UpdateHPCoRo(value));
    }

    public virtual IEnumerator UpdateHPCoRo(int value)
    {
        var newHPValue = CurrentHP + value;

        float elapsedTime = 0f;
        float lerpDuration = 1f;
        int valueToOutput;

        combatantUI.statsDisplay.HPTMPAnimator.SetTrigger("bump");

        while (elapsedTime < lerpDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            valueToOutput = Mathf.RoundToInt(Mathf.Lerp(CurrentHP, newHPValue, t));
            CurrentHP = valueToOutput;
            combatantUI.statsDisplay.UpdateHPDisplay(CurrentHP);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (CurrentHP == 0)
        {
            combatantUI.statsDisplay.statsDisplayContainerAnimator.Play("StatsDisplayOnDefeat");
            movementScript.animator.Play("Fall");
        }

        yield return new WaitForSeconds(0.5f);
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
