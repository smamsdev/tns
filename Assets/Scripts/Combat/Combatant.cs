using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combatant : MonoBehaviour
{
    public string combatantName;
    public CombatantUI combatantUI;
    public MovementScript movementScript;
    public GameObject fightingPosition;
    public bool isBackstabbed;
    public bool isEnclosed;
    public int CombatLookDirX
    {
        get => _combatLookDirX;
        set
        {
            _combatLookDirX = value;
            movementScript.animator.SetFloat("CombatLookDirX", value);
        }
    }

    [SerializeField] private int _combatLookDirX;

    public int AttackBase
    {
        get { return attackBase; }
        set
        {
            attackBase = Mathf.Clamp(value, 0, 999);
        }
    }

    public int FendBase
    {
        get { return fendBase; }
        set
        {
            fendBase = Mathf.Clamp(value, 0, 999);
        }
    }

    public int MaxHP
    {
        get { return maxHP; }
        set
        {
            maxHP = Mathf.Clamp(value, 0, 9999);
        }
    }

    public int CurrentHP
    {
        get => currentHP;
        set
        {
            currentHP = Mathf.Clamp(value, 0, 9999);
        }
    }

    [Header("Stats")]
    [SerializeField] private int attackBase;
    [SerializeField] private int fendBase;
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;

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

    [Header("Moves")]
    public Combatant targetCombatant;
    public Move moveSelected;

    public abstract void SelectMove();

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
}
